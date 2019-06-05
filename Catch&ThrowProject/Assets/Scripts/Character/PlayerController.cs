using System.Collections;
using System.Collections.Generic;
using Assets.Resources;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Rigidbody rigidbody;
    public Collider normalCollider;
    public SkinnedMeshRenderer playerMesh;
    public SkinnedMeshRenderer maskMesh;

    public Skin playerSkin;

    [Header("Classes")]
    public Shield shield;
    public InputController inputControl;
    public UpdatePlayerPanel uiPanel;
    private _LevelManager levelManager;
    public readonly StateMachine stateMachine = new StateMachine();

    [Header("States")]
    public Idle idleState;
    public Walk walkState;
    public Jump jumpState;
    public DoubleJump doubleJumpState;
    public Fall fallState;
    public Attack attackState;
    public Duck duckState;
    public DropOnPlatform dropOnPlatformState;
    public MashButton mashButtonState;

    public Dash dashState;
    public Catch catchState;
    public Caught caughtState;
    public Stun stunState;

    public Die dieState;

    [Header(("Other data"))]
    public int jumpLayer;
    public int normalLayer;

    [SerializeField] private float distanceToGround;
    [SerializeField] private LayerMask groundDetectionCollisions;
    [SerializeField] private float fallingSpeedThreshold;
    [SerializeField] private float downPlatformThreshold;
    [SerializeField] private float deathZoneTouchForceDown = 10f;
    [SerializeField] private float deathZoneTouchForceUp = 50f;
    [SerializeField] private float bounceUpOnDeathZoneThreshold;
    [SerializeField] private float collisionBoundariesMultiplier = 0.75f;

    public int actualAmmo { get; private set; } = 3;
    [SerializeField] private int initialAmmo;
    public int maxAmmo;
    //    [SerializeField] private float timeForAmmo;
    //    private IEnumerator recoverAmmo;


    [HideInInspector] public bool jumpMade;
    [HideInInspector] public bool impulseImpacts;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool isDead;
    private bool reloadAmmoinCourse;

    [HideInInspector] public Vector3 spawnPosition;

    public int health;

    public bool Invulnerable { get; set; }
    public bool CanMove { get; set; }

    public PlayerController caughtPlayer;


    #region MyRegion

    public PlayerController killer;

    public float secondsToResetKiller = 4f;
    public IEnumerator resetKiller;

    public Text ammo;

    #endregion

    private void Start()
    {
        //        recoverAmmo = RecoverAmmoOverTime(timeForAmmo);
        actualAmmo = initialAmmo;
        stateMachine.ChangeState(idleState);
        spawnPosition = transform.position;
        levelManager = _LevelManager.instance;
        resetKiller = ResetKiller();
    }

    private void Update()
    {
        if (levelManager.matchState != _LevelManager.MatchState.Playing) return;

        if (stateMachine.currentState == idleState || stateMachine.currentState == walkState ||
            stateMachine.currentState == fallState || stateMachine.currentState == attackState)
        {
            if (stateMachine.currentState == idleState || stateMachine.currentState == walkState)
                if (BellowDropThreshold() && inputControl.ButtonDown(InputController.Button.JUMP))
                {
                    ChangeState(dropOnPlatformState);
                    return;
                }
            if (inputControl.ButtonDown(InputController.Button.JUMP) && stateMachine.currentState != fallState) ChangeState(jumpState);
            if (inputControl.ButtonDown(InputController.Button.FIRE) && attackState.reloaded && stateMachine.currentState != attackState) ChangeState(attackState);
            if (inputControl.ButtonDown(InputController.Button.DASH) && dashState.available)
            {
                if (!CheckForGround()) ChangeState(dashState);
                else if (inputControl.Direction.y > downPlatformThreshold) ChangeState(dashState);
            }

        }

        if (inputControl.ButtonDown(InputController.Button.PAUSE)) levelManager.PauseGame(inputControl.controllerNumber);

        stateMachine.ExecuteState();
    }

    public void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public bool BellowDropThreshold()
    {
        return inputControl.Vertical < downPlatformThreshold;
    }

    public void HorizontalMove(float speed)
    {
        var horizontal = inputControl.Horizontal;
        var velocity = rigidbody.velocity;

        if (Mathf.Abs(horizontal) > 0)
        {
            OrientatePlayer(horizontal);

            if (!impulseImpacts) velocity.x = speed * horizontal;
            else velocity.x += horizontal * speed * Time.deltaTime;
        }
        else
        {
            if (!CheckForGround() && impulseImpacts) { }
            else { velocity.x = speed * horizontal; }
        }

        rigidbody.velocity = velocity;
    }

    public void OrientatePlayer(float horizontal)
    {
        transform.rotation = Quaternion.Euler(0, Mathf.Sign(horizontal) < 0 ? 180 : 0, 0);
        ammo.transform.localRotation = Quaternion.Euler(0, Mathf.Sign(horizontal) < 0 ? 180 : 0, 0);
    }

    public void VerticalMove(Vector3 direction, float force)
    {
        var velocity = rigidbody.velocity;

        velocity += direction * force * Time.deltaTime;
        velocity.y = velocity.y >= -fallingSpeedThreshold ? velocity.y : -fallingSpeedThreshold;

        rigidbody.velocity = velocity;
    }

    public void Impulse(Vector3 direction, float impulseForce, bool impulseIsImpacting)
    {
        rigidbody.velocity = direction * impulseForce;
        impulseImpacts = impulseIsImpacting;
    }

    public bool CheckForGround()
    {
        if (gameObject.layer != normalLayer) return onGround = false;

        var startingPos = transform.position;

        Vector3 leftRayPosition = startingPos + (Vector3.right * (transform.position.x - normalCollider.bounds.min.x)) * collisionBoundariesMultiplier;
        Vector3 rightRayPosition = startingPos + (Vector3.right * (transform.position.x - normalCollider.bounds.max.x)) * collisionBoundariesMultiplier;

        onGround = Physics.Raycast(startingPos, Vector3.down, distanceToGround, groundDetectionCollisions);
        onGround = !onGround ? Physics.Raycast(leftRayPosition, Vector3.down, distanceToGround, groundDetectionCollisions) : onGround;
        onGround = !onGround ? Physics.Raycast(rightRayPosition, Vector3.down, distanceToGround, groundDetectionCollisions) : onGround;

        if (onGround) impulseImpacts = false;

        return onGround;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (stateMachine.currentState.Equals(dieState)) return;
        if (other.gameObject.CompareTag("Death Zone")) HitByDeathZone(other);
        if (other.gameObject.CompareTag("Immediate Death Zone")) ChangeState(dieState);
        else if (other.gameObject.CompareTag("Cross Zone")) other.gameObject.GetComponent<CrossZone>().ObjectCross(this.transform);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Bounce Zone")) other.gameObject.GetComponent<BounceZone>().BounceObject(rigidbody, this);
    }

    private void HitByDeathZone(Collision deathZone)
    {
        if (shield.shieldDestroyed)
        {
            ChangeState(dieState);

        }
        else
        {
            var direction = (deathZone.contacts[0].point - transform.position).normalized;
            direction.z = 0;
            var force = Mathf.Sign(direction.y) < 0 ? deathZoneTouchForceUp : deathZoneTouchForceDown;
            if (direction.y > 0 && direction.y > bounceUpOnDeathZoneThreshold)
                direction.y = Mathf.Abs(direction.y);

            Impulse(-direction, force, true);
            shield.DestroyShield();
            ChangeState(stunState);
        }
    }

    public void ProjectileHit(Vector3 hitDirection, float hitForce, float damage)
    {
        if (!shield.shieldDestroyed) shield.Hit(damage);
        else
        {
            shield.ImpactBlink();
            stunState.stunByTime = true;
            if (CheckForGround()) hitDirection.y = 1;
            Impulse(hitDirection, hitForce, true);
            ChangeState(idleState);
        }
    }

    public void MeleeHit(float meleeDamage, Vector3 hitDirection, float hitForce, PlayerController other)
    {
        Impulse(hitDirection, hitForce, true);

        if (stateMachine.currentState == dashState)
        {
            ChangeState(stunState);
        }
        else
        {
            shield.Hit(meleeDamage);
        }

        other.stunState.stunByTime = true;
        other.ChangeState(stunState);
    }

    public bool PlayerHasAmmo()
    {
        return actualAmmo > 0;
    }

    public bool AmmoIsMax()
    {
        return actualAmmo >= maxAmmo;
    }

    public void ResupplyAmmo(int ammo)
    {
        actualAmmo += ammo;
        this.ammo.text = actualAmmo.ToString();
        this.ammo.enabled = true;

        StartCoroutine(DeactivateAmmoText());

        if (AmmoIsMax()) StopReloading();
    }

    private IEnumerator DeactivateAmmoText()
    {
        yield return new WaitForSeconds(1);
        if (stateMachine.currentState != attackState)
            this.ammo.enabled = false;
    }

    private void CallForReload()
    {
        reloadAmmoinCourse = true;
        //        recoverAmmo = RecoverAmmoOverTime(timeForAmmo);
        //        StartCoroutine(recoverAmmo);
    }

    private void StopReloading()
    {
        reloadAmmoinCourse = false;
    }

    public void ConsumeAmmo(int ammo)
    {
        actualAmmo -= ammo;
        if (actualAmmo < 0) actualAmmo = 0;
    }

    //    private bool CheckForRecoverAmmo()
    //    {
    //        if (reloadAmmoinCourse) return false;
    ////        return actualAmmo < maxAmmo;
    //    }

    //    private IEnumerator RecoverAmmoOverTime(float time)
    //    {
    //        var actualTime = 0f;
    //        uiPanel.SetAmmoFillActive(true);
    //        while (actualTime < time)
    //        {
    //            actualTime += Time.deltaTime;
    //            uiPanel.UpdateAmmoFill(actualTime, time);
    //            yield return null;
    //
    //        }
    //        uiPanel.SetAmmoFillActive(false);
    //        actualAmmo++;
    //        reloadAmmoinCourse = false;
    //    }

    public void RespawnAmmo()
    {
        actualAmmo = initialAmmo;
        StopReloading();
    }

    public float GetInitialAmmo()
    {
        return initialAmmo;
    }

    private IEnumerator ResetKiller()
    {
        yield return new WaitForSeconds(secondsToResetKiller);

        killer = null;
    }

    public void SetSkin(Skin skin)
    {
        playerSkin = skin;
        dashState.walkTrail.material.color = skin.mainColor;
        playerMesh.material.mainTexture = playerSkin.playerTexture;
        maskMesh.material.mainTexture = playerSkin.maskTexture;
    }

}

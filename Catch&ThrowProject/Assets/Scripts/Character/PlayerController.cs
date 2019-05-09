using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Rigidbody rigidbody;
    public Collider normalCollider;

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
    public DropOnPlatform DropOnPlatformState;

    public Dash dashState;
    public Catch catchState;
    public Caught caughtState;
    public Stun stunState;

    public Die dieState;

    [Header(("Other data"))]
    public int jumpLayer;
    public int normalLayer;

    public float sphereCollisionRadius;

    [SerializeField] private float distanceToGround;
    [SerializeField] private LayerMask groundDetectionCollisions;
    [SerializeField] private float fallingSpeedThreshold;
    [SerializeField] private float downPlatformThreshold;

    public int actualAmmo { get; private set; } = 3;
    [SerializeField] private int initialAmmo;
    [SerializeField] private float timeForAmmo;
    public int maxAmmo;
    private IEnumerator recoverAmmo;


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

    private void Start()
    {
        recoverAmmo = RecoverAmmoOverTime(timeForAmmo);
        actualAmmo = initialAmmo;
        stateMachine.ChangeState(idleState);
        spawnPosition = transform.position;
        levelManager = _LevelManager.instance;
    }

    private void Update()
    {
        if (levelManager.matchState != _LevelManager.MatchState.Playing) return;

        if (stateMachine.currentState == idleState || stateMachine.currentState == walkState ||
            stateMachine.currentState == fallState || stateMachine.currentState == attackState)
        {
            if (stateMachine.currentState == idleState || stateMachine.currentState == walkState)
                if (inputControl.ButtonDown(InputController.Button.JUMP) && inputControl.Vertical < downPlatformThreshold)
                {
                    ChangeState(DropOnPlatformState);
                    return;
                }
            if (inputControl.ButtonDown(InputController.Button.JUMP) && stateMachine.currentState != fallState) ChangeState(jumpState);
            if (inputControl.ButtonDown(InputController.Button.FIRE) && attackState.reloaded && stateMachine.currentState != attackState) ChangeState(attackState);
            if (inputControl.ButtonDown(InputController.Button.DASH) && dashState.available) ChangeState(dashState);

        }

        if (inputControl.ButtonDown(InputController.Button.PAUSE)) levelManager.PauseGame(inputControl.controllerNumber);

        stateMachine.ExecuteState();
    }

    private void FixedUpdate()
    {
        uiPanel.UpdateAmmoText(actualAmmo);
        if (!CheckForRecoverAmmo()) return;
        CallForReload();
    }

    public void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void HorizontalMove(float speed)
    {
        var horizontal = inputControl.Horizontal;
        var velocity = rigidbody.velocity;

        if (Mathf.Abs(horizontal) > 0)
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Sign(horizontal) < 0 ? 180 : 0, 0);

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

        Vector3 leftRayPosition = startingPos + Vector3.right * (transform.position.x - normalCollider.bounds.min.x);
        Vector3 rightRayPosition = startingPos + Vector3.right * (transform.position.x - normalCollider.bounds.max.x);

        onGround = Physics.Raycast(startingPos, Vector3.down, distanceToGround, groundDetectionCollisions);
        onGround = !onGround ? Physics.Raycast(leftRayPosition, Vector3.down, distanceToGround, groundDetectionCollisions) : onGround;
        onGround = !onGround ? Physics.Raycast(rightRayPosition, Vector3.down, distanceToGround, groundDetectionCollisions) : onGround;

        if (onGround) impulseImpacts = false;

        return onGround;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Death Zone")) ChangeState(dieState);
        else if (other.gameObject.CompareTag("Bounce Zone")) other.gameObject.GetComponent<BounceZone>().BounceObject(rigidbody, this);
        else if (other.gameObject.CompareTag("Cross Zone")) other.gameObject.GetComponent<CrossZone>().ObjectCross(this.transform);
    }

    public void ProjectileHit(Vector3 hitDirection, float hitForce, float damage)
    {
        if (!shield.shieldDestroyed) shield.Hit(damage);
        else
        {
            shield.ImpactBlink();
            ChangeState(stunState);
        }
    }

    public void MeleeHit(float meleeDamage, Vector3 hitDirection, float hitForce)
    {
        shield.Hit(meleeDamage);

        if (stateMachine.currentState == dashState)
        {
            Impulse(hitDirection, hitForce, true);
            ChangeState(stunState);
        }
    }

    public bool PlayerHasAmmo()
    {
        return actualAmmo > 0;
    }

    public void ResupplyAmmo(int ammo)
    {
        if (actualAmmo >= maxAmmo) return;
        actualAmmo += ammo;
        if (actualAmmo >= maxAmmo) StopReloading();
    }

    private void CallForReload()
    {
        reloadAmmoinCourse = true;
        recoverAmmo = RecoverAmmoOverTime(timeForAmmo);
        StartCoroutine(recoverAmmo);
    }

    private void StopReloading()
    {
        StopCoroutine(recoverAmmo);
        uiPanel.SetAmmoFillActive(false);
        reloadAmmoinCourse = false;
    }

    public void ConsumeAmmo(int ammo)
    {
        actualAmmo -= ammo;
        if (actualAmmo < 0) actualAmmo = 0;
    }

    public bool CheckForRecoverAmmo()
    {
        if (reloadAmmoinCourse) return false;
        return actualAmmo < maxAmmo;
    }

    private IEnumerator RecoverAmmoOverTime(float time)
    {
        var actualTime = 0f;
        uiPanel.SetAmmoFillActive(true);
        while (actualTime < time)
        {
            actualTime += Time.deltaTime;
            uiPanel.UpdateAmmoFill(actualTime, time);
            yield return null;

        }
        uiPanel.SetAmmoFillActive(false);
        actualAmmo++;
        reloadAmmoinCourse = false;
    }

    public void RespawnAmmo()
    {
        actualAmmo = initialAmmo;
        StopReloading();
    }
}

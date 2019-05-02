using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseState
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private float reloadTime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shootOffset;
    [SerializeField] private float shootRecoilForce;
    [SerializeField] private float shootDirectionThreshold;

    [HideInInspector] public bool reloaded = true;
    
    public Transform directionAffordance;
    [SerializeField] private float directionAffordanceDistance;
    [SerializeField] private float meleeDetectionDistance;
    [SerializeField] private float shootFallingSpeed;
    [SerializeField] private LayerMask meleeDetectionMask;

    private Vector3 lastDir;

    [SerializeField] private Transform meleeTrigger;
    [SerializeField] private float meleeAttackThreshold;
    [SerializeField] private float hitForce;
    [SerializeField] private float hitFriction;
    [SerializeField] private bool isPastMelee;
    private IEnumerator countMeleeFunction;
    
    public override void Enter()
    {
        countMeleeFunction = CountMeleeTime();
        StartCoroutine(countMeleeFunction);
        
        lastDir = transform.right;
        
//        if (playerController.onGround) playerController.rigidbody.velocity = Vector3.zero;
//        playerController.rigidbody.velocity = Vector3.zero;
//        if (reloaded) ShootProjectile();
//        else playerController.ChangeState(playerController.idleState);
    }

    public override void Execute()
    {
        if (!playerController.CheckForGround()) 
            playerController.VerticalMove(Vector3.down, shootFallingSpeed);
        else
        {
            playerController.rigidbody.velocity = Vector3.zero;
        }
        
        var actualDirection = (Vector3) playerController.inputControl.Direction.normalized;       
        var direction = lastDir ;
        if (actualDirection != Vector3.zero)
        {
            direction = actualDirection;
            lastDir = actualDirection;
        }

        if (direction == Vector3.zero) direction = transform.right;
        directionAffordance.position = transform.position + direction * directionAffordanceDistance;
        
        var rotationAffordanceZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        var rotationAffordance = Quaternion.Euler(0, 0, rotationAffordanceZ);
        directionAffordance.rotation = rotationAffordance;
        
        if (playerController.inputControl.ButtonIsUp(InputController.Button.FIRE))
        {
//            PlayerController enemyPlayer = null; // IsMelee(playerController.inputControl.Direction);
//            
//            if(enemyPlayer) HitMelee(enemyPlayer);
//            else ShootProjectile(direction);
            if (!isPastMelee)
            {
                HitMelee();
            }
            else
            {
                ShootProjectile(direction);
            }
        }        
    }

    private IEnumerator CountMeleeTime()
    {
        var actualFrameTime = 0f;
        isPastMelee = false;
        while (actualFrameTime <= meleeAttackThreshold)
        {
            
            yield return new WaitForEndOfFrame();
            actualFrameTime++;
        }
        
        directionAffordance.gameObject.SetActive(true);

        isPastMelee = true;
    }

    private PlayerController IsMelee(Vector3 direction)
    {
        PlayerController player = null;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction,out hit, meleeDetectionDistance, meleeDetectionMask))
        {
            player = hit.transform.GetComponent<PlayerController>();
        }

        return player;
    }

    public override void Exit()
    {
        if(countMeleeFunction != null) 
            StopCoroutine(countMeleeFunction);
        
        directionAffordance.gameObject.SetActive(false);
    }
    public void ShootProjectile(Vector2 direction)
    {
        print("Shoot");
//        var direction = playerController.inputControl.Direction.normalized;

  
        if (direction == Vector2.zero) direction = transform.right;

        var rotationZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0, 0, rotationZ);

        var speed = projectileSpeed;

        var projectileInstance = Instantiate(projectile, transform.position + (shootOffset * (Vector3)direction), rotation);
        var projectileClass = projectileInstance.GetComponent<Projectile>();
        projectileClass.SetBullet(direction, speed);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), projectileInstance.GetComponent<Collider>(), true);

        var force = Vector3.up * shootRecoilForce;
        if (Mathf.Abs(force.magnitude)> 0 && direction.y <= 0 && !playerController.onGround) playerController.rigidbody.velocity = force;

//        StartCoroutine(Reload(reloadTime));
        AttackFinished();
    }

    public void HitMelee()
    {
        print("Melee");

        StartCoroutine(ApplyHitForce());
        
        AttackFinished();

    }
    private IEnumerator ApplyHitForce()
    {
        meleeTrigger.gameObject.SetActive(true);
        var playerRb = playerController.rigidbody;
        var hitDir = transform.right.x;
        playerRb.velocity += Vector3.right * hitForce * Mathf.Sign(hitDir);
        var velocity = playerRb.velocity;
        
        while (playerRb.velocity.x > 0)
        {
            velocity = playerRb.velocity;

            yield return new WaitForEndOfFrame();

            velocity.x -= hitFriction * Mathf.Sign(hitDir) * Time.deltaTime;

            playerRb.velocity = velocity;

        }

        velocity.x = 0;
        playerRb.velocity = velocity;
        meleeTrigger.gameObject.SetActive(false);

        AttackFinished();
    }

    public void AttackFinished()
    {
        playerController.ChangeState(playerController.idleState);
    }

    private IEnumerator Reload(float time)
    {
        reloaded = false;

        yield return new WaitForSeconds(time);

        reloaded = true;
    }
}
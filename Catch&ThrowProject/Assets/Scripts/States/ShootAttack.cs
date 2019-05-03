using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : BaseState
{
    [SerializeField] private GameObject projectile;

    [Header("Shoot: ")]

    [SerializeField] private float reloadTime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shootOffset;
    [SerializeField] private float shootRecoilForce;
    [SerializeField] private float shootDirectionThreshold;

    [HideInInspector] public bool reloaded = true;
    
    [Header("Direction: ")]

    
    public Transform directionAffordance;
    [SerializeField] private float directionAffordanceDistance;
    [SerializeField] private LayerMask meleeDetectionMask;

    private Vector3 lastDir;
    
    [Header("Melee: ")]

    [SerializeField] private Transform meleeTrigger;
    [SerializeField] private float meleeDetectionDistance;
    [SerializeField] private float meleeHitDamage;
    [SerializeField] private float hitForce;
    [SerializeField] private float hitbackForce;
    
    public override void Enter()
    {
        directionAffordance.gameObject.SetActive(true);
        lastDir = transform.right;
        
//        if (playerController.onGround) playerController.rigidbody.velocity = Vector3.zero;
//        playerController.rigidbody.velocity = Vector3.zero;
//        else playerController.ChangeState(playerController.idleState);
    }

    public override void Execute()
    {
        if (!playerController.CheckForGround())
        {
            playerController.fallState.CheckForCrossingPlatforms();
            playerController.fallState.ExecuteFallSpeed();

        }
        else
        {
            var velocity = playerController.rigidbody.velocity;
            velocity.x = 0;
            playerController.rigidbody.velocity = velocity;
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
            PlayerController enemyPlayer = IsMelee(playerController.inputControl.Direction);

            if (enemyPlayer) HitMelee(enemyPlayer,direction);
            else ShootProjectile(direction);
        }
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
        directionAffordance.gameObject.SetActive(false);
    }
    public void ShootProjectile(Vector2 direction)
    {
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

    public void HitMelee(PlayerController enemyPlayer , Vector3 direction)
    {
        print("Melee");

//        StartCoroutine(ApplyHitForce());
        enemyPlayer.MeleeHit(meleeHitDamage,direction,hitForce);
        
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
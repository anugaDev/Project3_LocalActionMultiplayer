using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseState
{
    [SerializeField] private GameObject projectile;

    [Header("Shoot: ")]

    [SerializeField] private float reloadTime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shootOffset;
    [SerializeField] private float shootRecoilForce;

    [HideInInspector] public bool reloaded = true;
    
    [Header("Direction: ")]

    
    public Transform directionAffordance;
    [SerializeField] private float directionAffordanceDistance;

    private Vector3 lastDir;
    
    [Header("Melee: ")]

    [SerializeField] private float meleeDetectionAngle;
    [SerializeField] private float meleeDetectionDistance;
    [SerializeField] private LayerMask meleeDetectionMask;
    [SerializeField] private float meleeHitDamage;
    [SerializeField] private float hitForce;

    [SerializeField] private Color playerColor;

    
    public override void Enter()
    {
        directionAffordance.gameObject.SetActive(true);
        lastDir = transform.right.normalized;
        
//        if (playerController.onGround) playerController.rigidbody.velocity = Vector3.zero;
//        playerController.rigidbody.velocity = Vector3.zero;
//        else playerController.ChangeState(playerController.idleState);
    }

    public override void Execute()
    {
        
        #region UpdateMovement
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
        #endregion
        
        
        #region Direction Affordance
        var actualDirection = (Vector3) playerController.inputControl.Direction.normalized;       
        var direction = lastDir ;
        if (actualDirection != Vector3.zero)
        {
            direction = actualDirection;
            lastDir = actualDirection;
        }

        directionAffordance.position = transform.position + direction * directionAffordanceDistance;
        
        var rotationAffordanceZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        var rotationAffordance = Quaternion.Euler(0, 0, rotationAffordanceZ);
        directionAffordance.rotation = rotationAffordance;

        #endregion


        #region Attack
        if (playerController.inputControl.ButtonIsUp(InputController.Button.FIRE))
        {
            if (direction == Vector3.zero) direction = transform.right.normalized;
            PlayerController enemyPlayer = IsMelee(playerController.inputControl.Direction);


            if (enemyPlayer) HitMelee(enemyPlayer,direction);
            else
                if(playerController.PlayerHasAmmo())ShootProjectile(direction);
                else ActionWithoutAmmo();
        }
        #endregion
    }  

    private PlayerController IsMelee(Vector3 direction)
    {
        var detected = Physics.OverlapSphere(transform.position, meleeDetectionDistance, meleeDetectionMask);

        PlayerController playerReturn = null;
        
        foreach (var playerTransform in (detected))
        {
            var controller = playerTransform.gameObject.GetComponent<PlayerController>();
            if (controller == null) continue;
            if (controller == playerController) continue;
            if(!(Vector3.Angle((controller.transform.position - transform.position).normalized,direction) < meleeDetectionAngle) ) continue;
            
            
            if(playerReturn == null) playerReturn = controller;
            else if ((controller.transform.position - transform.position).magnitude <
                     (playerReturn.transform.position - transform.position).magnitude)
                playerReturn = controller;


        }

        return playerReturn;

    }
    public override void Exit()
    {
        directionAffordance.gameObject.SetActive(false);
    }
    public void ShootProjectile(Vector2 direction)
    {
      

        var speed = projectileSpeed;

        var projectileInstance = Instantiate(projectile, transform.position + (shootOffset * (Vector3)direction), Quaternion.identity);
        var projectileClass = projectileInstance.GetComponent<Projectile>();
        projectileClass.SetBullet(direction, speed, playerController,Color.red);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), projectileInstance.GetComponent<Collider>(), true);

        var force = Vector3.up * shootRecoilForce;
        if (Mathf.Abs(force.magnitude)> 0 && direction.y <= 0 && !playerController.onGround) playerController.rigidbody.velocity = force;
        
        playerController.ConsumeAmmo(1);

        AttackFinished();
    }

    public void HitMelee(PlayerController enemyPlayer , Vector3 direction)
    {

        enemyPlayer.MeleeHit(meleeHitDamage,direction,hitForce);
        
        AttackFinished();
    }

    private void ActionWithoutAmmo()
    {
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : BaseState
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shootOffset;
    public override void Enter()
    {
        GetController();
        
        ShootProjectile();
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }

    public void ShootProjectile()
    {
        var direction = playerController.inputControl.Direction;
        
        var rotationZ = Mathf.Atan2(direction.x,direction.y) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0,0,rotationZ);
       
        var projectileInstance = Instantiate(projectile, transform.position + (shootOffset * (Vector3)direction),rotation);
        var projectileClass = projectileInstance.GetComponent<Projectile>();
        
        projectileClass.SetBullet(direction,projectileSpeed);
        
        playerController.ChangeState(playerController.idleState);
    }
}

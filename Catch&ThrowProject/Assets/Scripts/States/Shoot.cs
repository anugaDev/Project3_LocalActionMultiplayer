﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : BaseState
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float reloadTime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shootOffset;
    private bool reloaded = true;
    public override void Enter()
    {
        GetController();
        
        if(reloaded) ShootProjectile();
        else playerController.ChangeState(playerController.idleState);
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
        direction.x = Mathf.Sign(direction.x) * Mathf.Ceil(Mathf.Abs(direction.x));
        direction.y = Mathf.Sign(direction.y) * Mathf.Ceil(Mathf.Abs(direction.y));
        if(direction == Vector2.zero) direction = transform.right;
        
        var rotationZ = Mathf.Atan2(direction.x,direction.y) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0,0,rotationZ);
       
        var speed = projectileSpeed;
        if (Mathf.Abs(direction.x) + Mathf.Abs(direction.y) > 1) speed /= 2;

        var projectileInstance = Instantiate(projectile, transform.position + (shootOffset * (Vector3)direction),rotation);
        var projectileClass = projectileInstance.GetComponent<Projectile>();
        projectileClass.SetBullet(direction,speed);
        
        StartCoroutine(Reload(reloadTime));
        playerController.ChangeState(playerController.idleState);

    }

    private IEnumerator Reload(float time)
    {
        reloaded = false;
        
        yield return new WaitForSeconds(time);

        reloaded = true;
    }
}

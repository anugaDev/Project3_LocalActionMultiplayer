using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : BaseState
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float reloadTime;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shootOffset;
    [SerializeField] private float shootRecoilForce;
    [SerializeField] private float shootDirectionThreshold;
    public bool reloaded = true;
    public override void Enter()
    {

        if (reloaded) ShootProjectile();
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

        var sign = Mathf.Sign(direction.x);
        direction.x = sign * ( Mathf.Abs(direction.x) >= shootDirectionThreshold ? 1 : 0 );
        sign = Mathf.Sign(direction.y);
        direction.y = sign * ( Mathf.Abs(direction.y) >= shootDirectionThreshold ? 1 : 0 );

        if (direction == Vector2.zero) direction = transform.right;

        var rotationZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0, 0, rotationZ);

        var speed = projectileSpeed;
//        if (Mathf.Abs(direction.x) + Mathf.Abs(direction.y) > 1) speed /= 2;

        var projectileInstance = Instantiate(projectile, transform.position + (shootOffset * (Vector3)direction), rotation);
        var projectileClass = projectileInstance.GetComponent<Projectile>();
        projectileClass.SetBullet(direction, speed);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), projectileInstance.GetComponent<Collider>(), true);

        var force = Vector3.up * shootRecoilForce;
        if (force.magnitude > 0 && direction.y <= 0 && !playerController.onGround) playerController.rigidbody.velocity = force;
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

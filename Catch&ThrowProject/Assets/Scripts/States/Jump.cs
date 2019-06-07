using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BaseState
{
    [SerializeField] private float jumpingSpeed;
    [HideInInspector] public bool commingFromJump;

    public override void Enter()
    {
        if (playerController.onGround) SetJumpForce();
        else playerController.ChangeState(playerController.fallState);
        
        base.Enter();
    }

    public override void Execute() { }

    public override void Exit() { }

    public void SetJumpForce()
    {
        var xVelocity = playerController.rigidbody.velocity.x;
        playerController.Impulse(Vector3.up,jumpingSpeed,false);
        playerController.rigidbody.velocity += Vector3.right * xVelocity;
        commingFromJump = true;
        playerController.ChangeState(playerController.fallState);
    }
}

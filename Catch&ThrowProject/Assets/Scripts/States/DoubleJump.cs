using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : BaseState
{
    [SerializeField] private float jumpingSpeed;
    public override void Enter()
    {
        if(!playerController.jumpMade)
            SetJumpForce();

        else
        {
            playerController.ChangeState(playerController.fallState);
        }

    }
    public override void Execute() { }

    public override void Exit()
    {
        
    }

    public void SetJumpForce()
    {
        playerController.rigidbody.velocity = Vector3.up * jumpingSpeed; 
        playerController.ChangeState(playerController.fallState);
        playerController.jumpMade = true;
    }
}

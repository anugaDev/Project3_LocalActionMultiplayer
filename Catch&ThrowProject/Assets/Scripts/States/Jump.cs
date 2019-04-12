using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BaseState
{
    [SerializeField] private float jumpingSpeed;

    public override void Enter()
    {
        if(playerController.onGround)
            SetJumpForce();
        else if (!playerController.jumpMade)
        {
            SetJumpForce();
            playerController.jumpMade = true;
        }

    }
    public override void Execute() { }

    public override void Exit()
    {
        
    }

    public void SetJumpForce()
    {
//        playerController.jumpMade = true;
        playerController.rigidbody.velocity = Vector3.up * jumpingSpeed; 
        playerController.ChangeState(playerController.fallState);
    }
}

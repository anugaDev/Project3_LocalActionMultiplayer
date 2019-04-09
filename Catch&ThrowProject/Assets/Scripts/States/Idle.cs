using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    public override void Enter()
    {
        GetController();

        playerController.jumpMade = false;
        
        playerController.rigidbody.velocity = Vector3.zero;
//        playerController.animator.SetTrigger(animationTrigger);
    }

    public override void Execute()
    {
        if (Math.Abs(playerController.inputControl.Horizontal) > 0)
            playerController.ChangeState(playerController.walkState);

        if(!playerController.surfaceColliding)
            playerController.ChangeState(playerController.fallState);
    }

    public override void Exit()
    {
        
    }

  
}

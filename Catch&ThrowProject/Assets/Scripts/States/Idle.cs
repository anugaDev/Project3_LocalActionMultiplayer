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

        if(!playerController.CheckForGround())
            playerController.ChangeState(playerController.fallState);
        
        if(playerController.inputControl.Vertical > 0 && !playerController.jumpMade)
            playerController.ChangeState(playerController.jumpState);
    }

    public override void Exit()
    {
        
    }

  
}

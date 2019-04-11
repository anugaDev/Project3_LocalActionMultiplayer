using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    public override void Enter()
    {
        GetController();

        
//        playerController.animator.SetTrigger(animationTrigger);
    }

    public override void Execute()
    {
        if (!playerController.CheckForGround())
            playerController.ChangeState(playerController.fallState);
        else playerController.jumpMade = false;
        
        if (Math.Abs(playerController.inputControl.Horizontal) > 0)
            playerController.ChangeState(playerController.walkState);
        
        if(playerController.inputControl.ButtonDown(InputController.Button.FIRE))
            playerController.ChangeState(playerController.shootState);
        
        if(playerController.inputControl.ButtonDown(InputController.Button.JUMP) && !playerController.jumpMade )
            playerController.ChangeState(playerController.jumpState);
    }

    public override void Exit()
    {
        
    }

  
}

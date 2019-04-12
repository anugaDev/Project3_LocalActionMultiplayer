using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    public override void Enter()
    {


        //        playerController.animator.SetTrigger(animationTrigger);
    }

    public override void Execute()
    {
        if (Mathf.Abs(playerController.inputControl.Horizontal) > 0) playerController.ChangeState(playerController.walkState);

    }

    public override void Exit()
    {

    }


}

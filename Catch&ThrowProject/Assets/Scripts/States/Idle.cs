using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    public override void Execute()
    {
        if (Mathf.Abs(playerController.inputControl.Horizontal) > 0) playerController.ChangeState(playerController.walkState);

        if (!playerController.CheckForGround()) playerController.ChangeState(playerController.fallState);
        else playerController.jumpMade = false;
    }
}

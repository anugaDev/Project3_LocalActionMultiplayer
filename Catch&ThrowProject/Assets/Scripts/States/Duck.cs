using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : BaseState
{
    public override void Enter()
    {
        playerController.rigidbody.velocity = Vector3.zero;
        base.Enter();
    }

    public override void Execute()
    {
        if(playerController.inputControl.ButtonDown(InputController.Button.JUMP))
            playerController.ChangeState(playerController.dropOnPlatformState);
        else if(!playerController.BellowDropThreshold())
            playerController.ChangeState(playerController.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}

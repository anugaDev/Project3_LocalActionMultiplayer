using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caught : BaseState
{
    public override void Enter()
    {
        playerController.CanMove = false;
        playerController.dashState.released = false;
    }

    public override void Execute()
    {
        playerController.rigidbody.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        gameObject.layer = playerController.normalLayer;
        playerController.CanMove = true;
    }
}

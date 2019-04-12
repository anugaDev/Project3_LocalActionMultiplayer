using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caught : BaseState
{
    public override void Enter()
    {
        playerController.Invulnerable = true;
        playerController.CanMove = false;
    }

    public override void Execute()
    {
        playerController.rigidbody.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }
}

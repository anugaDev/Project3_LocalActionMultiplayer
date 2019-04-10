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

    }

    public override void Exit()
    {
        playerController.Invulnerable = false;
        playerController.CanMove = true;
    }
}

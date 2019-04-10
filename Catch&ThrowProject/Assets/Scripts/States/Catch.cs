using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : BaseState
{
    [SerializeField] private readonly float timeBeforeThrow = 1.5f;
    [SerializeField] private readonly float throwForce = 10f;

    public override void Enter()
    {
        playerController.Invulnerable = true;
        playerController.CanMove = false;

        StartCoroutine(StopCatch());
    }

    public override void Execute()
    {
        if (playerController.inputControl.ButtonDown(InputController.Button.DASH)) ThrowPlayer(playerController.caughtPlayer);
    }

    public override void Exit()
    {
        playerController.Invulnerable = false;
        playerController.CanMove = true;
    }

    private void ThrowPlayer(PlayerController caughtPlayer)
    {
        StopCoroutine(StopCatch());

        Vector3 direction = playerController.inputControl.Direction;

        //Apply force to Enemy
        //Change Enemy State to Stun (?)

        //Apply inverse force to Player
        //Change Player State to Stun (?)
    }

    private IEnumerator StopCatch()
    {
        yield return new WaitForSeconds(timeBeforeThrow);

        //Change Enemy State to Fall (?)
        //Change Player State to Fall (?)
    }
}

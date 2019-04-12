using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : BaseState
{
    [SerializeField] private float timeBeforeThrow = 1.5f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float reactionForceMultiplier = 0.25f;

    public override void Enter()
    {
        playerController.rigidbody.velocity = Vector3.zero;

        playerController.Invulnerable = true;
        playerController.CanMove = false;

        StartCoroutine(StopCatch());
    }

    public override void Execute()
    {
        playerController.rigidbody.velocity = Vector3.zero;

        if (playerController.inputControl.ButtonDown(InputController.Button.DASH)) ThrowPlayer(playerController.caughtPlayer);
    }

    public override void Exit()
    {
        playerController.Invulnerable = false;
        playerController.CanMove = true;
        playerController.caughtPlayer = null;
        playerController.dashState.playerTrigger.isTrigger = false;
    }

    private void ThrowPlayer(PlayerController caughtPlayer)
    {
        StopAllCoroutines();

        Vector3 direction = playerController.inputControl.Direction;

        playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.stunState);
        playerController.caughtPlayer.rigidbody.velocity = direction * throwForce;

        playerController.ChangeState(playerController.stunState);
        playerController.rigidbody.velocity = -direction * throwForce * reactionForceMultiplier;
    }

    private IEnumerator StopCatch()
    {
        yield return new WaitForSeconds(timeBeforeThrow);

        playerController.ChangeState(playerController.fallState);
        playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.fallState);
    }
}

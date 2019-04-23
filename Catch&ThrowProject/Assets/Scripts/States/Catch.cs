using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : BaseState
{
    [SerializeField] private float timeBeforeThrow = 1.5f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float reactionForceMultiplier = 0.25f;

    [SerializeField] private GameObject directionMarker;
    [SerializeField] private float directionMarkerDistance = 5f;


    public override void Enter()
    {
        directionMarker.SetActive(true);

        playerController.rigidbody.velocity = Vector3.zero;

        playerController.Invulnerable = true;
        playerController.CanMove = false;

        StartCoroutine(StopCatch());
    }

    public override void Execute()
    {
        PositionateMarker();

        playerController.rigidbody.velocity = Vector3.zero;

        if (playerController.inputControl.ButtonDown(InputController.Button.DASH))
        {
            StopAllCoroutines();

            ThrowPlayer(playerController.caughtPlayer);
        }
    }

    public override void Exit()
    {
        playerController.Invulnerable = false;
        playerController.CanMove = true;
        playerController.caughtPlayer = null;
        playerController.dashState.playerTrigger.isTrigger = false;

        directionMarker.SetActive(false);
    }

    private void ThrowPlayer(PlayerController caughtPlayer)
    {
        Vector3 direction = playerController.inputControl.Direction;

        if (direction == Vector3.zero) direction = transform.right;

        playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.stunState);
        playerController.caughtPlayer.rigidbody.velocity = direction * throwForce;

        playerController.ChangeState(playerController.stunState);
        playerController.rigidbody.velocity = -direction * throwForce * reactionForceMultiplier;
    }

    private void PositionateMarker()
    {
        Vector3 direction = new Vector3(playerController.inputControl.Direction.x, 
                                        playerController.inputControl.Direction.y, 
                                        0).normalized;

        directionMarker.transform.position = transform.position + direction * directionMarkerDistance;
    }

    private IEnumerator StopCatch()
    {
        yield return new WaitForSeconds(timeBeforeThrow);

        if (playerController.caughtPlayer) playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.fallState);

        playerController.ChangeState(playerController.fallState);
    }
}

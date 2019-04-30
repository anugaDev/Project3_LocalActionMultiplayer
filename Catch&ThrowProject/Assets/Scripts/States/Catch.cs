using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catch : BaseState
{
    [SerializeField] private float timeBeforeThrow = 1.5f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float minThrowForce = 1f;
    [SerializeField] private float reactionForceMultiplier = 0.25f;

    [SerializeField] private GameObject directionMarker;
    [SerializeField] private float directionMarkerDistance = 5f;

    private float timer = 0f;

    private float maxSizeX = 4.5f;
    private float minSizeX = 1f;

    private float maxSizeY = 1.5f;
    private float minSizeY = 1f;

    public override void Enter()
    {
        directionMarker.SetActive(true);

        playerController.rigidbody.velocity = Vector3.zero;

        playerController.Invulnerable = true;
        playerController.CanMove = false;
    }

    public override void Execute()
    {
        PositionateMarker();
        StopCatch();

        playerController.rigidbody.velocity = Vector3.zero;

        if (playerController.inputControl.ButtonDown(InputController.Button.DASH))
        {
            StopAllCoroutines();

            ThrowPlayer(playerController.caughtPlayer);
        }
    }

    public override void Exit()
    {
        timer = 0;

        playerController.Invulnerable = false;
        playerController.CanMove = true;
        playerController.caughtPlayer = null;
        playerController.dashState.playerTrigger.isTrigger = false;

        directionMarker.SetActive(false);
    }

    private void ThrowPlayer(PlayerController caughtPlayer)
    {
        Vector3 direction = playerController.inputControl.Direction;

        float force = Mathf.Max(throwForce * (1 - (timer / timeBeforeThrow)), minThrowForce);
        print(force);

        if (direction == Vector3.zero) direction = transform.right;

        playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.stunState);
        playerController.caughtPlayer.rigidbody.velocity = direction * force;

        playerController.ChangeState(playerController.stunState);
        playerController.rigidbody.velocity = -direction * force * reactionForceMultiplier;
    }

    private void PositionateMarker()
    {
        Vector3 direction = new Vector3(-playerController.inputControl.Direction.x,
                                        playerController.inputControl.Direction.y,
                                        0).normalized;

        float playerRotation = transform.rotation.eulerAngles.y == 0 ? 1 : -1;

        float angle = Vector3.SignedAngle(playerRotation * transform.right, direction, Vector3.back);

        directionMarker.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void StopCatch()
    {
        timer += Time.deltaTime;

        ResizeArrow();

        if (timer >= timeBeforeThrow)
        {
            if (playerController.caughtPlayer) playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.fallState);

            playerController.ChangeState(playerController.fallState);
        }
    }

    private void ResizeArrow()
    {
        float xSize = maxSizeX - ((maxSizeX - minSizeX) * (timer / timeBeforeThrow));
        float ySize = maxSizeY - ((maxSizeY - minSizeY) * (timer / timeBeforeThrow));

        directionMarker.transform.localScale = new Vector3(xSize, ySize, directionMarker.transform.localScale.z);
    }
}

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

    private Vector3 ThrowDirection;

    public override void Enter()
    {

        playerController.gameObject.layer = playerController.normalLayer;
        playerController.caughtPlayer.gameObject.layer = playerController.caughtPlayer.normalLayer;


        directionMarker.SetActive(true);

        playerController.rigidbody.velocity = Vector3.zero;

        playerController.dashState.released = false;
        playerController.Invulnerable = true;
        playerController.CanMove = false;

        _LevelManager.instance.cameraFollow.CatchMode = true;
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
        playerController.gameObject.layer = playerController.normalLayer;
        timer = 0;
        playerController.dashState.released = true;
        playerController.Invulnerable = false;
        playerController.CanMove = true;
        playerController.caughtPlayer = null;
        playerController.dashState.playerTrigger.isTrigger = false;

        directionMarker.SetActive(false);

        _LevelManager.instance.cameraFollow.CatchMode = false;
    }

    private void ThrowPlayer(PlayerController caughtPlayer)
    {
        playerController.gameObject.layer = caughtPlayer.normalLayer;
        float force = Mathf.Max(throwForce * (1 - (timer / timeBeforeThrow)), minThrowForce);

        if (ThrowDirection == Vector3.zero) ThrowDirection = transform.right;

        if (-ThrowDirection.y >= 0)
        {
            playerController.gameObject.layer = caughtPlayer.jumpLayer;
        }

        caughtPlayer.stunState.stunByTime = false;
        if(ThrowDirection.y > 0) caughtPlayer.gameObject.layer = caughtPlayer.jumpLayer;
        caughtPlayer.Impulse(ThrowDirection, force, true);
        caughtPlayer.ChangeState(playerController.caughtPlayer.stunState);


        playerController.ChangeState(playerController.stunState);
        if (!playerController.CheckForGround()) playerController.Impulse(-ThrowDirection, force * reactionForceMultiplier, false);
    }

    private void PositionateMarker()
    {
        if (playerController.inputControl.Direction.x == 0 && playerController.inputControl.Direction.y == 0) return;

        Vector3 direction = new Vector3(-playerController.inputControl.Direction.x,
                                        playerController.inputControl.Direction.y,
                                        0).normalized;

        ThrowDirection = playerController.inputControl.Direction;

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

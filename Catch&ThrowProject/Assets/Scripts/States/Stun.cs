using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : BaseState
{
    public float threshHoldSpeed = 2.5f;

    public float breakSpeed = 200f;

    public override void Enter()
    {
        playerController.gameObject.layer = playerController.jumpLayer;
    }

    public override void Execute()
    {
        if (playerController.rigidbody.velocity.magnitude <= threshHoldSpeed) playerController.ChangeState(playerController.fallState);
        else
        {
            Vector3 breakVector = new Vector3(playerController.rigidbody.velocity.x > 0 ? -1 : 1, -1, 0);

            playerController.rigidbody.velocity += breakVector * breakSpeed * Time.deltaTime;
        }
    }

    public override void Exit()
    {
        playerController.CanMove = true;
        playerController.Invulnerable = false;
    }
}

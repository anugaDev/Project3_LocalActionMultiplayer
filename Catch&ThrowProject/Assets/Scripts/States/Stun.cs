using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : BaseState
{
    public float threshHoldSpeed = 2.5f;
    public float breakSpeed = 200f;

    public bool stunByTime = false;

    public float stunTime = 1.5f;
    private float timer = 0f;

    public override void Enter()
    {
        playerController.gameObject.layer = playerController.jumpLayer;
    }

    public override void Execute()
    {
        if (stunByTime)
        {
            timer += Time.deltaTime;

            if (timer >= stunTime)
            {
                timer = 0f;

                playerController.ChangeState(playerController.fallState);
            }
        }
        else
        {
            if (playerController.rigidbody.velocity.magnitude <= threshHoldSpeed) playerController.ChangeState(playerController.fallState);
            else
            {
                Vector3 breakVector = new Vector3(playerController.rigidbody.velocity.x > 0 ? -1 : 1, -1, 0);

                playerController.rigidbody.velocity += breakVector * breakSpeed * Time.deltaTime;
            }
        }
    }

    public override void Exit()
    {
        playerController.CanMove = true;
        playerController.Invulnerable = false;
    }
}

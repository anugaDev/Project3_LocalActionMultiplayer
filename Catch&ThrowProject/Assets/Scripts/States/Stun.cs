﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : BaseState
{
    public float threshHoldSpeed = 2.5f;

    public float breakSpeed = 200f;

    public override void Execute()
    {
        if (playerController.rigidbody.velocity.magnitude <= threshHoldSpeed) playerController.ChangeState(playerController.fallState);
        else playerController.rigidbody.velocity += -playerController.rigidbody.velocity.normalized * breakSpeed * Time.deltaTime;
    }

    public override void Exit()
    {
        playerController.CanMove = true;
        playerController.Invulnerable = false;
    }
}

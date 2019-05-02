using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BaseState
{
    [SerializeField] private float jumpingSpeed;
    [HideInInspector] public bool commingFromJump;

    public override void Enter()
    {
        if (playerController.onGround) SetJumpForce();
        else playerController.ChangeState(playerController.fallState);
    }

    public override void Execute() { }

    public override void Exit() { }

    public void SetJumpForce()
    {
        playerController.Impulse(Vector3.up,jumpingSpeed);
        commingFromJump = true;
        playerController.ChangeState(playerController.fallState);
    }
}

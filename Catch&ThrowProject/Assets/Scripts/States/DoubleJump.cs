using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : BaseState
{
    [SerializeField] private float jumpingSpeed;
    [HideInInspector] public bool commingFromJump;
    public override void Enter()
    {
        if(!playerController.jumpMade)
            SetJumpForce();

        else
        {
            playerController.ChangeState(playerController.fallState);
        }

    }
    public override void Execute() { }

    public override void Exit()
    {
        
    }

    public void SetJumpForce()
    {
        playerController.Impulse(Vector3.up,jumpingSpeed);
        playerController.jumpMade = true;
        commingFromJump = true;
        playerController.ChangeState(playerController.fallState);
    }
}

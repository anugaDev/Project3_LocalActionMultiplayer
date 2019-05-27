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
        base.Enter();
    }

    public override void Execute() { }

    public override void Exit() { }

    public void SetJumpForce()
    {
        soundEvent.start();
        playerController.Impulse(Vector3.up,jumpingSpeed,false);
        commingFromJump = true;
        playerController.ChangeState(playerController.fallState);
    }
}

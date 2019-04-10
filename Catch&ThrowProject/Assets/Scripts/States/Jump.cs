using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : BaseState
{
    [SerializeField] private float jumpingSpeed;

    public override void Enter()
    {
        GetController();
        SetJumpForce();

    }
    public override void Execute() { }

    public override void Exit()
    {
        
    }

    public void SetJumpForce()
    {
        playerController.rigidbody.velocity = Vector3.up * jumpingSpeed; 
        playerController.jumpMade = true;
        playerController.ChangeState(playerController.fallState);
    }
}

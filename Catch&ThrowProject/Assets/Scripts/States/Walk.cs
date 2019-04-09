using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : BaseState
{
    [SerializeField] private float walkSpeed;

    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        playerController.HorizontalMove(walkSpeed);
        
//        playerController.ChangeState(playerController.jumpState);
        
        if(Mathf.Abs(playerController.inputControl.Horizontal) == 0 )
            playerController.ChangeState(playerController.idleState);
        
        if(!playerController.surfaceColliding)
            playerController.ChangeState(playerController.fallState);
        
    }
    public override void Exit() { }
}

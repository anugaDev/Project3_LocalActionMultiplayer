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
        
        playerController.ChangeState(playerController.jumpState);
        
    }
    public override void Exit() { }
}

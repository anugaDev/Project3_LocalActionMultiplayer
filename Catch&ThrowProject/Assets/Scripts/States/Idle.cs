using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    
    public override void Enter()
    {
        GetController();
        
        playerController.animator.SetTrigger(animationTrigger);
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}

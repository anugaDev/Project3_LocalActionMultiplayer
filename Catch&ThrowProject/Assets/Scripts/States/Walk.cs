using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : BaseState
{
    [SerializeField] private float walkSpeed;

    public override void Enter()
    {
        GetController();
    }

    public override void Execute()
    {
        #region Stateupdate
        
        playerController.HorizontalMove(walkSpeed);
        
        #endregion
        
        #region ChangeConditions
        
        if(playerController.rigidbody.velocity.magnitude <=0.1f)
            playerController.ChangeState(playerController.idleState);
        
        if(!playerController.CheckForGround())
            playerController.ChangeState(playerController.fallState);
        
        if(playerController.inputControl.Vertical > 0 && !playerController.jumpMade)
            playerController.ChangeState(playerController.jumpState);
        
        #endregion
        
    }
    public override void Exit() { }
}

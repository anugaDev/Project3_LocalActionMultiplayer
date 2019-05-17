using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : BaseState
{
    [SerializeField] private float walkSpeed;

<<<<<<< HEAD
    public override void Enter()
    {
        base.Enter();
    }
=======
    public override void Enter() { }
>>>>>>> 739bf929bfe232e0281175e6b69a920782623b5d

    public override void Execute()
    {
        #region Stateupdate

        playerController.HorizontalMove(walkSpeed);

        #endregion

        #region ChangeConditions

        if (!playerController.CheckForGround()) playerController.ChangeState(playerController.fallState);
        if (playerController.rigidbody.velocity.magnitude <= 0.1f) playerController.ChangeState(playerController.idleState);

        #endregion
    }

<<<<<<< HEAD
    public override void Exit()
    {
        base.Exit();
    }
=======
    public override void Exit() { }
>>>>>>> 739bf929bfe232e0281175e6b69a920782623b5d
}

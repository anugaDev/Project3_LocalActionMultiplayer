using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : BaseState
{
    [SerializeField] private float walkSpeed;

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
}

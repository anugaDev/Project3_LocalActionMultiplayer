using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : BaseState
{
    [SerializeField] private float reEscaledSize = 0.75f;
    private float colliderSize;
    public override void Enter()
    {
        colliderSize = playerController.normalCollider.height;
        playerController.rigidbody.velocity = Vector3.zero;
        playerController.normalCollider.height *= reEscaledSize;
//        playerController.normalCollider.center += Vector3.down * reEscaledSize;
        base.Enter();

    }

    public override void Execute()
    {
        playerController.HorizontalMove(0);
        if(playerController.inputControl.ButtonDown(InputController.Button.JUMP))
            playerController.ChangeState(playerController.dropOnPlatformState);
        else if(!playerController.BellowDropThreshold())
            playerController.ChangeState(playerController.idleState);
    }

    public override void Exit()
    {
        playerController.normalCollider.height = colliderSize;
        playerController.normalCollider.center = Vector3.zero;

        base.Exit();
    }
}

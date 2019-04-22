using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : BaseState
{
    [SerializeField] private float pressingFallingSpeed;
    [SerializeField] private float notPressingFallingSpeed;
    [SerializeField] private float fallingSpeedThreshold;
    [SerializeField] private float glideSpeed;
    [SerializeField] private float fallPressedMultiply;
    [SerializeField] private LayerMask checkPlatformsLayerMask;
    private bool stopPressing;
    private bool groundHit;
    private float actualFallingSpeed;
    private float fallMultiply;

    private float firstEnter;
    public override void Enter()
    {

        firstEnter = Time.frameCount;
        gameObject.layer = playerController.jumpLayer;
        groundHit = false;

        if (playerController.inputControl.ButtonIsPressed(InputController.Button.JUMP))
        {
            stopPressing = false;

        }

        fallMultiply = 1;

    }

    public override void Execute()
    {
        #region StateUpdate

        if (playerController.rigidbody.velocity.y < 0 && gameObject.layer == playerController.jumpLayer)
        {
            if (Physics.OverlapSphere(transform.position, playerController.sphereCollisionRadius,
                    checkPlatformsLayerMask).Length <= 0)
                gameObject.layer = playerController.normalLayer;
            else print("colliding");

        }
        
        actualFallingSpeed = ManageFallSpeed();
        var velocity = playerController.rigidbody.velocity;
        fallMultiply = playerController.inputControl.Vertical < 0 ? fallPressedMultiply : 1;
        velocity += Vector3.down * actualFallingSpeed * fallMultiply * Time.deltaTime;
        velocity.y = velocity.y <= fallingSpeedThreshold ? velocity.y : -fallingSpeedThreshold;

        playerController.rigidbody.velocity = velocity;
        playerController.HorizontalMove(glideSpeed);

        #endregion

        #region ChangeConditions

        if (playerController.CheckForGround())// && gameObject.layer  == LayerMask.GetMask(LayerMask.LayerToName(playerController.normalLayer)))
        {
            playerController.ChangeState(playerController.idleState);
            groundHit = true;
        }

        if (playerController.inputControl.ButtonDown(InputController.Button.JUMP) && !playerController.jumpMade && Time.frameCount != firstEnter)
            playerController.ChangeState(playerController.doubleJumpState);

        #endregion
    }

    public override void Exit()
    {
        if (!groundHit) return;
        var velocity = playerController.rigidbody.velocity;
        velocity.y = 0;
        playerController.rigidbody.velocity = velocity;
        playerController.jumpMade = false;

    }

    private float ManageFallSpeed()
    {
        var speed = notPressingFallingSpeed;

        if (stopPressing) return speed;

        if (playerController.inputControl.ButtonIsPressed(InputController.Button.JUMP) && playerController.rigidbody.velocity.y > 0)
        {
            speed = pressingFallingSpeed;
        }
        else
        {
            stopPressing = true;
        }




        return speed;

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position,playerController.sphereCollisionRadius);

    }
}

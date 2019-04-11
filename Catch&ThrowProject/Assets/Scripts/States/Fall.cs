using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : BaseState
{
    [SerializeField] private  float pressingFallingSpeed;
    [SerializeField] private float notPressingFallingSpeed;
    [SerializeField] private float fallingSpeedThreshold;
    [SerializeField] private float glideSpeed;
    private bool stopPressing;
    private bool groundHit;
    private float actualFallingSpeed;
    private int fallMultiply;

    public override void Enter()
    {
        GetController();

        groundHit = false;
        
        if (playerController.inputControl.ButtonIsPressed(InputController.Button.JUMP))stopPressing = false;
        
        fallMultiply = 1;

    }

    public override void Execute()
    {
        #region StateUpdate

        actualFallingSpeed = ManageFallSpeed();
        var velocity = playerController.rigidbody.velocity;
//        fallMultiply = playerController.inputControl.Vertical < 0 ? 2 : 1;
        velocity += Vector3.down * actualFallingSpeed * fallMultiply;
//        velocity.y = velocity.y >= fallingSpeedThreshold ? velocity.y : -fallingSpeedThreshold;
        
        playerController.rigidbody.velocity = velocity;
        playerController.HorizontalMove(glideSpeed);
     
        #endregion
     
       #region ChangeConditions

       if(playerController.inputControl.ButtonDown(InputController.Button.JUMP)  && !playerController.jumpMade)
           playerController.ChangeState(playerController.jumpState);
       
       if(playerController.inputControl.ButtonDown(InputController.Button.FIRE))
           playerController.ChangeState(playerController.shootState);

       if (playerController.CheckForGround())
       {
           playerController.ChangeState(playerController.idleState);
           groundHit = true;

       }
           
       #endregion
       }

    public override void Exit()
    {
        if (!groundHit) return;
        var velocity = playerController.rigidbody.velocity;
        velocity.y = 0;
        playerController.rigidbody.velocity = velocity;
        playerController.jumpMade = true;
    }

    private float ManageFallSpeed()
    {
        var speed = notPressingFallingSpeed;

        if (stopPressing) return speed;
        
        if (playerController.inputControl.ButtonIsPressed(InputController.Button.JUMP) ) // && playerController.rigidbody.velocity.y > 0)
        {
            speed = pressingFallingSpeed;
        }
        else
        {
            stopPressing = true;
        }




        return speed;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : BaseState
{
    [SerializeField] private float pressingFallingSpeed;
    [SerializeField] private float notPressingFallingSpeed;
    [SerializeField] private float fallingSpeedThreshold;
    [SerializeField] private float glideSpeed;
    private bool stopPressing;
    private float actualFallingSpeed;
    private int fallMultiply;

    public override void Enter()
    {
        
        GetController();
        
        if (playerController.inputControl.ButtonIsPressed(InputController.Button.JUMP))stopPressing = false;
        
        fallMultiply = 1;

    }

    public override void Execute()
    {
        #region StateUpdate

        actualFallingSpeed = ManageFallSpeed();
        
        fallMultiply = playerController.inputControl.Vertical < 0 ? 2 : 1;
        
//        playerController.rigidbody.AddForce(fallingSpeed * Vector3.down *  fallMultiply,ForceMode.Acceleration); 
        
        playerController.rigidbody.velocity += Vector3.down * actualFallingSpeed * fallMultiply; 

        
              
        playerController.HorizontalMove(glideSpeed);
     
        #endregion
     
       #region ChangeConditions

       if(playerController.inputControl.ButtonDown(InputController.Button.JUMP)  && !playerController.jumpMade)
           playerController.ChangeState(playerController.jumpState);
       
       if(playerController.inputControl.ButtonDown(InputController.Button.FIRE))
           playerController.ChangeState(playerController.shootState);
       
       if(playerController.CheckForGround())
           playerController.ChangeState(playerController.idleState);

       #endregion
       
    }

    public override void Exit()
    {
        var velocity = playerController.rigidbody.velocity;
        velocity.y = 0;
        playerController.rigidbody.velocity = velocity;
    }

    private float ManageFallSpeed()
    {
        print(playerController.inputControl.Vertical);

        
        var speed = notPressingFallingSpeed;

        if (stopPressing) return speed;
        
        if (playerController.inputControl.ButtonIsPressed(InputController.Button.JUMP))
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : BaseState
{
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float fallingSpeedThreshold;
    [SerializeField] private float glideSpeed;
    private float fallMultiply;

    public override void Enter()
    {
        GetController();

        fallMultiply = 1;

    }

    public override void Execute()
    {
        #region StateUpdate

        fallMultiply = playerController.inputControl.Vertical < 0 ? 2 : 1;
        
        playerController.rigidbody.AddForce(fallingSpeed * Vector3.down *  fallMultiply,ForceMode.Acceleration); 
              
        playerController.HorizontalMove(glideSpeed);
     
        #endregion
     
       #region ChangeConditions

       if(playerController.inputControl.Vertical > 0  && !playerController.jumpMade)
           playerController.ChangeState(playerController.jumpState);
       
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

    private void OnCollisionEnter(Collision other)
    {
        
    }
}

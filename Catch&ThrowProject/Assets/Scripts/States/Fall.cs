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


        

//        velocity = velocity.y < -fallingSpeedThreshold ? originalVelocity : velocity;


        playerController.rigidbody.AddForce(fallingSpeed * Vector3.down,ForceMode.Acceleration); 
       
//        print(playerController.rigidbody.velocity);
        
        playerController.HorizontalMove(glideSpeed);

        fallMultiply = playerController.inputControl.Vertical < 0 ? 2 : 1;

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
        playerController.rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}

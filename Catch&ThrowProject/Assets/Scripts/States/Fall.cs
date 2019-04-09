﻿using System.Collections;
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

        var originalVelocity = playerController.rigidbody.velocity;
        var velocity = originalVelocity;
        
        velocity -= Vector3.up  *(fallingSpeed * fallMultiply);

//        velocity = velocity.y < -fallingSpeedThreshold ? originalVelocity : velocity;
        
        playerController.rigidbody.velocity -= Vector3.up  *(fallingSpeed * fallMultiply);
       
        print(playerController.rigidbody.velocity);
        
        playerController.HorizontalMove(glideSpeed);

        fallMultiply = playerController.inputControl.Vertical < 0 ? 2 : 1;

        #endregion
     
       #region ChangeConditions

       if(playerController.inputControl.Vertical > 0  && !playerController.jumpMade)
           playerController.ChangeState(playerController.jumpState);
       

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

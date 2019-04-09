using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : BaseState
{
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float fallingSpeedThreshold;

    public override void Enter()
    {
        GetController();

    }

    public override void Execute()
    {
       playerController.rigidbody.velocity -= Vector3.up * fallingSpeed;
    }
    public override void Exit() { }
}

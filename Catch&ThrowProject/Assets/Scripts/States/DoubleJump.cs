using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : BaseState
{
    [SerializeField] private float jumpingSpeed;
    [HideInInspector] public bool commingFromJump;

    public GameObject jumpParticles;

    public override void Enter()
    {
        Instantiate(jumpParticles, transform.position - Vector3.up, Quaternion.identity);

        if (!playerController.jumpMade) SetJumpForce();

        else
        {
            playerController.ChangeState(playerController.fallState);
            return;
        }

        base.Enter();
    }

    public void SetJumpForce()
    {
        playerController.Impulse(Vector3.up, jumpingSpeed, false);
        playerController.jumpMade = true;
        commingFromJump = true;
        playerController.ChangeState(playerController.fallState);
    }
}

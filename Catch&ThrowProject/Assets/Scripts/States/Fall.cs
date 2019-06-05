using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fall : BaseState
{
    [SerializeField] private float pressingFallingSpeed;
    [SerializeField] private float notPressingFallingSpeed;
    [SerializeField] private float glideSpeed;
    [SerializeField] private float fallPressedMultiply;
    [SerializeField] private float multiplyFallThreshold;

    [SerializeField] private LayerMask checkPlatformsLayerMask;
    [SerializeField] private float sphereCollisionRadius = 0.92f;


    private bool stopPressing;
    private bool groundHit;

    private float actualFallingSpeed;
    private float fallMultiply;
    private float firstEnter;
    
    [FMODUnity.EventRef] public string landSound;
    private FMOD.Studio.EventInstance soundEventLand;

    [Header("Gizmos")] 
    
    [SerializeField] private bool showDetectionRadius;

    private void Start()
    {
        soundEventLand = FMODUnity.RuntimeManager.CreateInstance(landSound);
    }

    public override void Enter()
    {
        base.Enter();

        firstEnter = Time.frameCount;
        if(IsNotCrossingPlatforms()) gameObject.layer = playerController.jumpLayer;
        groundHit = false;

        if (playerController.inputControl.ButtonIsPressed(InputController.Button.JUMP) &&
           (playerController.jumpState.commingFromJump || playerController.doubleJumpState.commingFromJump))
        {
            stopPressing = false;
            playerController.jumpState.commingFromJump = false;
            playerController.doubleJumpState.commingFromJump = false;
        }
        else
        {
            stopPressing = true;
        }

        fallMultiply = 1;
    }


    public override void Execute()
    {
        #region StateUpdate

        CheckForCrossingPlatforms();

        ExecuteFallSpeed();
        playerController.HorizontalMove(glideSpeed);


        #endregion

        #region ChangeConditions

        if (playerController.CheckForGround())
        {
            groundHit = true;
            playerController.ChangeState(playerController.idleState);
        }

        if (playerController.inputControl.ButtonDown(InputController.Button.JUMP) && !playerController.jumpMade && Time.frameCount != firstEnter)
            playerController.ChangeState(playerController.doubleJumpState);

        #endregion
    }

    public void CheckForCrossingPlatforms()
    {
        if (playerController.rigidbody.velocity.y < 0 && gameObject.layer == playerController.jumpLayer)
        {
            if (IsNotCrossingPlatforms())
                gameObject.layer = playerController.normalLayer;
        }
    }

    public bool IsNotCrossingPlatforms()
    {
        return !Physics.OverlapSphere(transform.position, sphereCollisionRadius,
                checkPlatformsLayerMask).Any();
    }

    public void ExecuteFallSpeed()
    {
        actualFallingSpeed = ManageFallSpeed();

        fallMultiply = playerController.inputControl.Vertical < -multiplyFallThreshold ? fallPressedMultiply : 1;

        playerController.VerticalMove(Vector3.down, actualFallingSpeed * fallMultiply);

    }

    public override void Exit()
    {
        base.Exit();

        if (!groundHit) return;

        var velocity = playerController.rigidbody.velocity;
        velocity.y = 0;

        playerController.rigidbody.velocity = velocity;
        playerController.jumpMade = false;

        soundEventLand.start();
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
        
        if(showDetectionRadius) Gizmos.DrawSphere(transform.position, sphereCollisionRadius);
    }
}

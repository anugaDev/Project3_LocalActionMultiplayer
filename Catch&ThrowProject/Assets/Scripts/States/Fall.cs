using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fall : BaseState
{
   


    [SerializeField] private float pressingFallingSpeed;
    [SerializeField] private float notPressingFallingSpeed;
    [SerializeField] private float glideAcceleration;
    [SerializeField] private float fallPressedMultiply;
    [SerializeField] private float multiplyFallThreshold;


    [SerializeField] private float FallRotation = -10f;
    [SerializeField] private float FallRotationSpeed = 60f;

    private Transform playerFBX;

    [SerializeField] private LayerMask checkPlatformsLayerMask;

    [Header("Animation Bools")]
    [SerializeField] public string animationFallingBool;
    [SerializeField] public string groundedBool;
    
    private bool stopPressing;
    private bool groundHit;

    private float actualFallingSpeed;
    private float fallMultiply;
    private float firstEnter;

    [FMODUnity.EventRef] public string landSound;
    private FMOD.Studio.EventInstance soundEventLand;

    private void Start()
    {
        soundEventLand = FMODUnity.RuntimeManager.CreateInstance(landSound);
    }

    public override void Enter()
    {
        playerController.animator.SetBool(groundedBool, false);


        base.Enter();

        firstEnter = Time.frameCount;
        gameObject.layer = playerController.jumpLayer;
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
        playerController.HorizontalMove(glideAcceleration);

        UpdatePlayerRotation();

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

    private void UpdatePlayerRotation()
    {
        if (!playerFBX) playerFBX = playerController.playerMesh.transform.parent;

        var actualRotation = playerFBX.rotation.eulerAngles;

        bool goingUp = playerController.rigidbody.velocity.y > 0;
        actualRotation.z = (goingUp ? -FallRotation : 0);

        float rotationSpeed = FallRotationSpeed * Time.deltaTime;

        playerFBX.rotation = Quaternion.RotateTowards(playerFBX.rotation, Quaternion.Euler(actualRotation), rotationSpeed);
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
        return !Physics.OverlapSphere(transform.position, playerController.sphereCollisionRadius,
                checkPlatformsLayerMask).Any();
    }

    public void ExecuteFallSpeed()
    {
        playerController.animator.SetBool(animationFallingBool, playerController.rigidbody.velocity.y < 0 ? true : false);
        actualFallingSpeed = ManageFallSpeed();

        fallMultiply = playerController.inputControl.Vertical < -multiplyFallThreshold ? fallPressedMultiply : 1;

        playerController.VerticalMove(Vector3.down, actualFallingSpeed * fallMultiply);

    }

    public override void Exit()
    {
        base.Exit();

        if (!groundHit) return;

        playerController.animator.SetBool(groundedBool, true);
        var velocity = playerController.rigidbody.velocity;
        velocity.y = 0;

        playerController.rigidbody.velocity = velocity;
        playerController.jumpMade = false;

        playerController.playerMesh.transform.parent.rotation = Quaternion.Euler(0, playerController.playerMesh.transform.parent.rotation.eulerAngles.y, 0);

        playerController.animator.SetBool(animationFallingBool, false);


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

    //    private void OnDrawGizmos()
    //    {
    //        Gizmos.DrawSphere(transform.position, playerController.sphereCollisionRadius);
    //    }
}

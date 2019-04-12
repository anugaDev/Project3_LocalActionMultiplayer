using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    public Animator animator;
    public Rigidbody rigidbody;
    public Transform skillObject;
    public Collider normalCollider;

    [Header("Classes")]
    public Shield shield;
    public InputController inputControl;
    private readonly StateMachine stateMachine = new StateMachine();


    [Header("States")]
    public Idle idleState;
    public Walk walkState;
    public Jump jumpState;
    public Fall fallState;
    public Shoot shootState;

    public Dash dashState;
    public Catch catchState;
    public Caught caughtState;


    public int jumpLayer;
    public int normalLayer;
    public float sphereCollisionRadius;
    [HideInInspector] public bool jumpMade;
    [HideInInspector] public bool onGround;
    [SerializeField] private float distanceToGround;
    [SerializeField] private LayerMask groundDetectionCollisions;

    public bool Invulnerable { get; set; }
    public bool CanMove { get; set; }

    public PlayerController caughtPlayer;

    private void Start()
    {
        stateMachine.ChangeState(idleState);
    }

    private void Update()
    {
        if (stateMachine.currentState == idleState || stateMachine.currentState == walkState)
        {
            if (inputControl.ButtonDown(InputController.Button.JUMP)) ChangeState(jumpState);
            if (inputControl.ButtonDown(InputController.Button.FIRE)) ChangeState(shootState);
            if (inputControl.ButtonDown(InputController.Button.DASH)) ChangeState(dashState);

            if (!CheckForGround()) ChangeState(fallState);
            else jumpMade = false;
        }

        stateMachine.ExecuteState();
    }

    public void ChangeState(BaseState newState)
    {
        print("State :" + newState);

        stateMachine.ChangeState(newState);
    }

    public void HorizontalMove(float speed)
    {
        var velocity = rigidbody.velocity;
        velocity.x = speed * inputControl.Horizontal;
        rigidbody.velocity = velocity;
    }
    public bool CheckForGround()
    {
        onGround = Physics.Raycast(transform.position, -Vector3.up, distanceToGround, groundDetectionCollisions);

        return onGround;
    }
    private void OnCollisionEnter(Collision other)
    {
        switch (other.transform.tag)
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    public Animator animator;
    public Rigidbody rigidbody;
    public Transform skillObject;
    public Collider skillCollider;

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

    public Catch catchState;
    public Caught caughtState;

    [HideInInspector] public bool jumpMade;
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
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround, groundDetectionCollisions);
    }

}

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

    [HideInInspector] public bool jumpMade;
    public bool surfaceColliding { get; private set; }
    [SerializeField] private float distanceToGround;

    private void Start()
    {
        stateMachine.ChangeState(idleState);
//        skillCollider = skillObject.GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        stateMachine.ExecuteState();

    }

    public void ChangeState(BaseState newState)
    {
        print("Switch state to: "+ newState);
        stateMachine.ChangeState(newState);
    }

    public void HorizontalMove(float speed)
    {
        var velocity = rigidbody.velocity;

        velocity.x = speed * inputControl.Horizontal;
        
        rigidbody.velocity = velocity;
    }
    private void OnCollisionStay(Collision other)
    {
        surfaceColliding = true;
    }

    public bool CheckForGround()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
    }
    
}

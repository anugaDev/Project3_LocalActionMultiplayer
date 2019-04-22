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
    public readonly StateMachine stateMachine = new StateMachine();


    [Header("States")]
    public Idle idleState;
    public Walk walkState;
    public Jump jumpState;
    public DoubleJump doubleJumpState;
    public Fall fallState;
    public Shoot shootState;

    public Dash dashState;
    public Catch catchState;
    public Caught caughtState;
    public Stun stunState;

    public Die dieState;


    //public int facing { private set; get; } = 1;
    public int jumpLayer;
    public int normalLayer;
    public float sphereCollisionRadius;
    [HideInInspector] public bool jumpMade;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool isDead;
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
        if (stateMachine.currentState == idleState || stateMachine.currentState == walkState || stateMachine.currentState == fallState)
        {
            if (inputControl.ButtonDown(InputController.Button.JUMP) && stateMachine.currentState != fallState) ChangeState(jumpState);
            if (inputControl.ButtonDown(InputController.Button.FIRE)) ChangeState(shootState);
            if (inputControl.ButtonDown(InputController.Button.DASH) && dashState.available) ChangeState(dashState);
        }

        stateMachine.ExecuteState();
    }

    public void ChangeState(BaseState newState)
    {
        //        print("State :" + newState);

        stateMachine.ChangeState(newState);
    }

    public void HorizontalMove(float speed)
    {
        var horizontal = inputControl.Horizontal;

        if (Mathf.Abs(horizontal) > 0)
        {
            var rotation = Quaternion.Euler(0, Mathf.Sign(horizontal) < 0 ? 180 : 0, 0);
            transform.rotation = rotation;
        }
        var velocity = rigidbody.velocity;
        velocity.x = speed * horizontal;
        rigidbody.velocity = velocity;
    }
    public bool CheckForGround()
    {
        onGround = Physics.Raycast(transform.position, -Vector3.up, distanceToGround, groundDetectionCollisions);

        if (gameObject.layer != normalLayer) onGround = false;

        
        return onGround;
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Death Zone")
            ChangeState(dieState);
        if (other.gameObject.tag == "Projectile")
        {
            var proj = other.transform.GetComponent<Projectile>();
            shield.Hit(proj.damage);
            if (shield.shieldDestroyed)
            {
                ChangeState(stunState); 
            }
            
        }
            

    }
}

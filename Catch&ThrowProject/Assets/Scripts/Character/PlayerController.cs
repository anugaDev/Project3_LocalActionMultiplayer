using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    public Animator animator;
    public Rigidbody rigidbody;
    public Transform directionAffordance;
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


    [Header( ("Other data"))]
    public int jumpLayer;
    public int normalLayer;
    public float sphereCollisionRadius;
    [SerializeField] private float distanceToGround;
    [SerializeField] private LayerMask groundDetectionCollisions;
    [SerializeField] private float directionAffordanceDistance;
    [HideInInspector] public bool jumpMade;
    [HideInInspector] public bool onGround;
    [HideInInspector] public bool isDead;
    [HideInInspector] public Vector3 spawnPosition;

    public bool Invulnerable { get; set; }
    public bool CanMove { get; set; }

    public PlayerController caughtPlayer;

    private void Start()
    {
        stateMachine.ChangeState(idleState);
        spawnPosition = transform.position;
    }

    private void Update()
    {
        if (stateMachine.currentState == idleState || stateMachine.currentState == walkState || stateMachine.currentState == fallState)
        {
            if (inputControl.ButtonDown(InputController.Button.JUMP) && stateMachine.currentState != fallState) ChangeState(jumpState);
            if (inputControl.ButtonIsPressed(InputController.Button.FIRE) && shootState.reloaded) ChangeState(shootState);
            if (inputControl.ButtonDown(InputController.Button.DASH) && dashState.available) ChangeState(dashState);
        }

        var direction = (Vector3) inputControl.RightDirection;
        if (direction == Vector3.zero) direction =  transform.right;
        directionAffordance.position = transform.position + direction * directionAffordanceDistance;

        stateMachine.ExecuteState();
    }

    public void ChangeState(BaseState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void HorizontalMove(float speed)
    {
        var horizontal = inputControl.Horizontal;

        if (Mathf.Abs(horizontal) > 0)
        {
            var rotation = Quaternion.Euler(0, Mathf.Sign(horizontal) < 0 ? 180 : 0, 0);
            transform.rotation = rotation;
            
            var direction = (Vector3) inputControl.RightDirection;
            if (direction == Vector3.zero) direction =  transform.right;
            directionAffordance.position = transform.position +( direction * directionAffordanceDistance);
       
            var rotationAffordanceZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            var rotationAffordance = Quaternion.Euler(0, 0, rotationAffordanceZ);
//            var rotationAffordance = Quaternion.LookRotation(direction, Vector3.up);
            directionAffordance.rotation = rotationAffordance;
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
        if (other.gameObject.CompareTag("Death Zone"))
        {
            ChangeState(dieState);
//            CameraUtilities.instance.MoveCamera(transform.position - other.contacts[0].point); 
        }
        else if(other.gameObject.CompareTag("Bounce Zone"))
        {
            other.gameObject.GetComponent<BounceZone>().BounceObject(rigidbody,this);
        }
        else if (other.gameObject.CompareTag("Cross Zone"))
        {
            other.gameObject.GetComponent<CrossZone>().ObjectCross(this.transform);
        }
            
            

    }

    public void ProjectileHit(Projectile projectile)
    {
        var proj = projectile.transform.GetComponent<Projectile>();
        if(!shield.shieldDestroyed) 
            shield.Hit(proj.damage);
        else
        {
            ChangeState(stunState); 
        }
        
    }
}

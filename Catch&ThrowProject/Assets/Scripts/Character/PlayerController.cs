﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public Rigidbody rigidbody;
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
    public Attack attackState;

    public Dash dashState;
    public Catch catchState;
    public Caught caughtState;
    public Stun stunState;

    public Die dieState;

    [Header(("Other data"))]
    public int jumpLayer;
    public int normalLayer;

    public float sphereCollisionRadius;
    public int actualAmmo { get; private set; } = 3;

    [SerializeField] private float distanceToGround;
    [SerializeField] private LayerMask groundDetectionCollisions;
    [SerializeField] private float airFriction;
    [SerializeField] private float fallingSpeedThreshold;
    [SerializeField] private float airGroundFriction;
    [SerializeField] private int maxAmmo;
    private float lastAirImpulseX;

    [HideInInspector] public bool jumpMade;
    [HideInInspector] public bool exitFromCaught;
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
            if (inputControl.ButtonDown(InputController.Button.FIRE) && attackState.reloaded) ChangeState(attackState);
            if (inputControl.ButtonDown(InputController.Button.DASH) && dashState.available) ChangeState(dashState);
        }
        
        stateMachine.ExecuteState();
      
    }

    public void ChangeState(BaseState newState)
    {
        print("New State :"+ newState);
        
        stateMachine.ChangeState(newState);
    }

    public void HorizontalMove(float speed)
    {
        var horizontal = inputControl.Horizontal;
        var velocity = rigidbody.velocity;

        if (Mathf.Abs(horizontal) > 0)
        {
            var rotation = Quaternion.Euler(0, Mathf.Sign(horizontal) < 0 ? 180 : 0, 0);
            transform.rotation = rotation;
            
            velocity.x = speed * horizontal;
        }
        
        else
        {
            if (!onGround)
            {
                var absVelocity = Mathf.Abs(velocity.x);
                absVelocity-= (airFriction * Mathf.Sign(lastAirImpulseX)) * Time.deltaTime;
                if (absVelocity < 0f)
                    absVelocity = 0;

                velocity.x = absVelocity;
            }

            else
            {
                if (exitFromCaught)
                    exitFromCaught = false;
                velocity.x = speed * horizontal;

            }
//                velocity.x -= (airGroundFriction * Mathf.Sign(transform.right.x)) * Time.deltaTime;

        }

        velocity.x = speed * horizontal;
        rigidbody.velocity = velocity;
        print(horizontal);
    }

    public void VerticalMove(Vector3 direction, float force)
    {
        var velocity = rigidbody.velocity;
        
        velocity += direction * force * Time.deltaTime;
        velocity.y = velocity.y >= -fallingSpeedThreshold ? velocity.y : -fallingSpeedThreshold;
        
        rigidbody.velocity = velocity;




    }

    public void Impulse( Vector3 direction,float impulseForce)
    {
        rigidbody.velocity = direction * impulseForce;
        lastAirImpulseX = rigidbody.velocity.x;
    }

    public bool CheckForGround()
    {
        if (gameObject.layer != normalLayer) return onGround = false;

//        var startingPos = new Vector3(0,normalCollider.bounds.min.y,0);
        var startingPos = transform.position;
        onGround = Physics.Raycast(startingPos, -Vector3.up, distanceToGround, groundDetectionCollisions);
//        onGround = !onGround ? Physics.Raycast(startingPos, -Vector3.up + Vector3.right, distanceToGround, groundDetectionCollisions): onGround;
//        onGround = !onGround ? Physics.Raycast(startingPos, -Vector3.up + Vector3.left, distanceToGround, groundDetectionCollisions): onGround;


        return onGround;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Death Zone"))
        {
            ChangeState(dieState);
        }
        else if (other.gameObject.CompareTag("Bounce Zone"))
        {
            other.gameObject.GetComponent<BounceZone>().BounceObject(rigidbody, this);
        }
        else if (other.gameObject.CompareTag("Cross Zone"))
        {
            other.gameObject.GetComponent<CrossZone>().ObjectCross(this.transform);
        }
    }

    public void ProjectileHit(Projectile projectile)
    {
        var proj = projectile.transform.GetComponent<Projectile>();

        if (!shield.shieldDestroyed) shield.Hit(proj.damage);
        else
        {
            shield.ImpactBlink();
            ChangeState(stunState);
        }
    }

    public void ResupplyAmmo(int ammo)
    {
        if (actualAmmo >= maxAmmo) return;
        actualAmmo += ammo;
        Mathf.Clamp(actualAmmo, 0, maxAmmo);

    }
}

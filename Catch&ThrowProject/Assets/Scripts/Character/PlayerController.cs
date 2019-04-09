using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    public Animator animator;
    public Rigidbody rigidbody;
    public Transform skillObject;
    [HideInInspector]public Collider skillCollider;
    
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

    private void Start()
    {
        stateMachine.ChangeState(idleState);
        skillCollider = skillObject.GetComponent<Collider>();
    }

    private void Update()
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
        rigidbody.velocity = Vector3.right * (inputControl.Horizontal * speed);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    public Animator animator;
    public Rigidbody rigidbody;
    
    [Header("Classes")]
    
    [HideInInspector] public StateMachine stateMachine = new StateMachine();
    public Shield shield;
    public InputController inputControl;
    
    [Header("States")]
    
    public Idle idleState;
    public Walk walkState;
    public Jump jumpState;
    public Fall fallState;
    public Shoot shootState;

    private void Start()
    {
        stateMachine.ChangeState(idleState);
    }

    private void Update()
    {
        stateMachine.ExecuteState();
    }
}

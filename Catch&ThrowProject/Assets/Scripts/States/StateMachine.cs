using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public BaseState currentState;

    public void ChangeState(BaseState nextState)
    {
        if (currentState != null) currentState.Exit();

        currentState = nextState;
        currentState.Enter();
    }

    public void ExecuteState()
    {
        if (currentState != null) currentState.Execute();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private StateMachine stateMachine = new StateMachine();

    private void Update()
    {
        stateMachine.ExecuteState();
    }
}

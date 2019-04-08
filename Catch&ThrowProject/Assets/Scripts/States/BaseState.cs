using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    public PlayerController playerController;

 
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}

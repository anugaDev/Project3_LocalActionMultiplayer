using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected string animationTrigger;
 
    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }


}

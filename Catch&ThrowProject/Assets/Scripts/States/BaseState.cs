using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected string animationBool;
    [SerializeField] protected string animationTrigger;

    public virtual void Enter()
    {
        if (animationBool != "") playerController.animator.SetBool(animationBool, true);
        if (animationTrigger != "") playerController.animator.SetTrigger(animationTrigger);
    }
    public virtual void Execute() { }

    public virtual void Exit()
    {
        if (animationBool != "") playerController.animator.SetBool(animationBool, false);
    }
}

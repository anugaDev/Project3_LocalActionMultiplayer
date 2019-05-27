using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected string animationBool;
    [SerializeField] protected string animationTrigger;

    [FMODUnity.EventRef]    
    [SerializeField] protected string enterSound;
    protected FMOD.Studio.EventInstance soundEvent;

    private void Start()
    {
        if (enterSound != "") soundEvent = FMODUnity.RuntimeManager.CreateInstance(enterSound);
    }
    
    public virtual void Enter()
    {
        if (animationBool != "") playerController.animator.SetBool(animationBool, true);
        if (animationTrigger != "") playerController.animator.SetTrigger(animationTrigger);
        if (enterSound != "") soundEvent.start();
    }
    public virtual void Execute() { }

    public virtual void Exit()
    {
        if (animationBool != "") playerController.animator.SetBool(animationBool, false);
    }
}

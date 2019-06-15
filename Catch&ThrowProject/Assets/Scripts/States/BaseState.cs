using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    [SerializeField] protected PlayerController playerController;
    [SerializeField] protected string animationBool;
    [SerializeField] protected string animationTrigger;

    [FMODUnity.EventRef] public string enterSound;
    [HideInInspector] public FMOD.Studio.EventInstance soundEventEnter;
    
    [FMODUnity.EventRef] public string exitSound;
    [HideInInspector] public FMOD.Studio.EventInstance soundEventExit;


    private void Start()
    {
        if (enterSound != "") soundEventEnter = FMODUnity.RuntimeManager.CreateInstance(enterSound);
        if(exitSound != "") soundEventExit = FMODUnity.RuntimeManager.CreateInstance(exitSound);
    }
    public virtual void Enter()
    {
        if (enterSound != "") soundEventEnter.start();
        if (animationBool != "") playerController.animator.SetBool(animationBool, true);
        if (animationTrigger != "") playerController.animator.SetTrigger(animationTrigger);
    }
    public virtual void Execute() { }

    public virtual void Exit()
    {
        if (exitSound !="") soundEventExit.start();
        if (animationBool != "") playerController.animator.SetBool(animationBool, false);
    }
}

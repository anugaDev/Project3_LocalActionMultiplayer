using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class MashButton : BaseState
{
    [SerializeField] private Transform mashAffordance;
    [SerializeField] private Transform scaledMashAffordance;
    [SerializeField] private int timesToMash = 10;
    [SerializeField] private float failTime = 10f;
    [SerializeField] private float backOutTimeForce = 20f;
    private float actualMashedTimes;

    
    [FMODUnity.EventRef] public string mashSucces;
    [FMODUnity.EventRef] public string mashFail;


    // Start is called before the first frame update
    public override void Enter()
    {
        playerController.rigidbody.velocity = Vector3.zero;
        actualMashedTimes = 0;
        StartCoroutine(CountForFail(failTime));
        mashAffordance.gameObject.SetActive(true);
        scaledMashAffordance.localScale = Vector3.one * actualMashedTimes;
    }

    public override void Execute()
    {
        if (playerController.inputControl.ButtonDown(InputController.Button.DASH))
        {
            actualMashedTimes++;
            scaledMashAffordance.localScale = Vector3.one * (actualMashedTimes/timesToMash);

        }
        
        if (actualMashedTimes >= timesToMash)
        {
            RuntimeManager.PlayOneShot(mashSucces);
            playerController.ChangeState(playerController.catchState);
            playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.caughtState);
        }
            
    }

    public override void Exit()
    {
        mashAffordance.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    IEnumerator CountForFail(float time)
    {
        yield return new WaitForSeconds(time);
        
        var dir = (playerController.caughtPlayer.transform.position - playerController.transform.position).normalized;
        playerController.Impulse(dir, backOutTimeForce, true);
        playerController.caughtPlayer.Impulse(-dir, backOutTimeForce, true);

        playerController.ChangeState(playerController.fallState);
        playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.fallState);
        RuntimeManager.PlayOneShot(mashFail);
    }

    public void RotateAffordance(float angle )
    {
        mashAffordance.localRotation = Quaternion.Euler( Vector3.up * angle);
    }
}

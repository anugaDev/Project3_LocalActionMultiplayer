using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashButton : BaseState
{
    [SerializeField] private Transform mashAffordance;
    [SerializeField] private Transform scaledMashAffordance;
    [SerializeField] private int timesToMash = 10;
    [SerializeField] private float failTime = 10f;
    private float actualMashedTimes;

    
    
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
        playerController.ChangeState(playerController.idleState);
        playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.idleState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashButton : BaseState
{
    [SerializeField] private int timesToMash = 10;
    [SerializeField] private float failTime = 10f;
    private float actualMashedTimes;
    private IEnumerator countFail;

    
    
    // Start is called before the first frame update
    public override void Enter()
    {
        actualMashedTimes = 0;
        countFail = CountForFail(failTime);
        StartCoroutine(countFail);
    }

    public override void Execute()
    {
        if (playerController.inputControl.ButtonDown(InputController.Button.DASH))
            actualMashedTimes++;
        
        if (actualMashedTimes >= timesToMash)
        {
            playerController.ChangeState(playerController.catchState);
            playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.caughtState);
        }
            
    }

    public override void Exit()
    {
        StopCoroutine(countFail);
    }

    IEnumerator CountForFail(float time)
    {
        yield return new WaitForSeconds(time);
        playerController.ChangeState(playerController.idleState);
        playerController.caughtPlayer.ChangeState(playerController.caughtPlayer.idleState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnPlatform : BaseState
{
    [SerializeField] private float checkPlatformDistance;
    [SerializeField] private float timeToSwitchFall;
    [SerializeField] private LayerMask checkPlatformMask;
    private IEnumerator checkTimeForExitDrop;
 
    public override void Enter()
    {
        if (CheckIfPlatformBellow())
        {
            playerController.gameObject.layer = playerController.jumpLayer;
            checkTimeForExitDrop = ChangeToFall(timeToSwitchFall);
            StartCoroutine(checkTimeForExitDrop);
        }
        else if(playerController.CheckForGround())
            playerController.ChangeState(playerController.jumpState);
        else
            playerController.ChangeState(playerController.fallState);
        
    }

    public override void Execute()
    {
        playerController.fallState.ExecuteFallSpeed();
    }

    public override void Exit()
    {
        if (checkTimeForExitDrop != null)
        {
            StopCoroutine(checkTimeForExitDrop);
            checkTimeForExitDrop = null;
        }
    }

    private bool CheckIfPlatformBellow()
    {
        var startingPos = transform.position;
        bool found = Physics.Raycast(startingPos, Vector3.down, checkPlatformDistance,
            checkPlatformMask);
        return found;
    }

    private IEnumerator ChangeToFall(float time)
    {
        yield return new WaitForSeconds(time);
        playerController.ChangeState(playerController.fallState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : BaseState
{
    [SerializeField] private Renderer playerRenderer;
    private IEnumerator delegateRespawn;
    
    public override void Enter()
    {
        GetKilled();
        playerController.isDead = true;
        playerController.rigidbody.isKinematic = true;
        playerController.normalCollider.enabled = false;

    }
    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        if (ReferenceEquals(delegateRespawn, null))
        {
            StopCoroutine(delegateRespawn);
            playerRenderer.enabled = true;
        }
        playerController.rigidbody.isKinematic = false;
        playerController.normalCollider.enabled = true;
    }

    public void GetKilled()
    {
        playerRenderer.enabled = false;
        delegateRespawn = GetRespawn(1);
        StartCoroutine(delegateRespawn);
       

        


    }

    IEnumerator GetRespawn(float time)
    {
        yield return new WaitForSeconds(time);
        
        Respawn();
    }

    public void Respawn()
    {
        playerController.isDead = false;
        transform.position = Vector3.zero;
        playerRenderer.enabled = true;
        playerController.gameObject.layer = playerController.normalLayer;
        
        playerController.ChangeState(playerController.idleState);
    }


}

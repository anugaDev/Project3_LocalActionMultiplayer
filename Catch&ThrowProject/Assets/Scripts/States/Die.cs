using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : BaseState
{
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private float cameraShakeForce;
    [SerializeField] private float cameraShakeTime;
    private IEnumerator delegateRespawn;
    
    public override void Enter()
    {
        CameraUtilities.instance.ShakeCamera(cameraShakeTime,cameraShakeForce);
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
        playerController.shield.ResetShield();
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
        transform.position = playerController.spawnPosition;
        var rotation = Vector3.zero;
        rotation.y = transform.position.x > 0 ? 180 : 0;
        transform.rotation = Quaternion.Euler(rotation);
        playerRenderer.enabled = true;
        playerController.gameObject.layer = playerController.normalLayer;
        playerController.shield.ResetShield();
        
        playerController.ChangeState(playerController.idleState);
    }


}

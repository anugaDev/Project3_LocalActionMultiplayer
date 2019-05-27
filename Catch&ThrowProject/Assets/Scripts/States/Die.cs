using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : BaseState
{
    [SerializeField] private GameObject playerModel;
    [SerializeField] private float cameraShakeForce;
    [SerializeField] private float cameraShakeTime;
    private IEnumerator delegateRespawn;

    public override void Enter()
    {
        CameraUtilities.instance.ShakeCamera(cameraShakeTime, cameraShakeForce);
        GetKilled();
        playerController.isDead = true;
        playerController.normalCollider.enabled = false;

        _LevelManager.instance.OnPlayerKilled(playerController);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        if (delegateRespawn == null)
        {
            StopCoroutine(delegateRespawn);
            playerModel.SetActive(true);
        }

        playerController.normalCollider.enabled = true;
        playerController.shield.ResetShield();
    }

    public void GetKilled()
    {
        _LevelManager.instance.cameraFollow.objectsToShow.Remove(this.transform);
        playerController.shield.DestroyShield();
        playerModel.SetActive(false);
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
        playerController.gameObject.layer = playerController.normalLayer;
        playerController.rigidbody.velocity = Vector3.zero;
        playerController.RespawnAmmo();
        playerController.isDead = false;
        _LevelManager.instance.SpawnPlayer(gameObject, null);
        _LevelManager.instance.cameraFollow.objectsToShow.Add(this.transform);
        var rotation = Vector3.zero;
        rotation.y = transform.position.x > 0 ? 180 : 0;
        transform.rotation = Quaternion.Euler(rotation);
        playerModel.SetActive(true);
        playerController.shield.ResetShield();

        playerController.ChangeState(playerController.idleState);
    }
    
}

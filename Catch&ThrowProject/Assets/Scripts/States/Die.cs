﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : BaseState
{
    [SerializeField] private GameObject playerModel;
    [SerializeField] private float cameraShakeForce;
    [SerializeField] private float cameraShakeTime;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject spawnParticles;
    [SerializeField] private float timeToRespawn = 1f;
    private IEnumerator delegateRespawn;

    public override void Enter()
    {
        CameraUtilities.instance.ShakeCamera(cameraShakeTime, cameraShakeForce);
        GetKilled();
        playerController.isDead = true;
        playerController.normalCollider.enabled = false;

        _LevelManager.instance.OnPlayerKilled(playerController);

        base.Enter();
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
        ParticleSystem particles = Instantiate(deathParticles, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        particles.startColor = playerController.playerSkin.mainColor;
        playerController.shield.DestroyShield();
        playerController.shield.StopAllCoroutines();
        playerModel.SetActive(false);
        playerController.dashState.available = false;
        playerController.dashState.walkTrail.enabled = false;
        playerController.throwParticles.gameObject.SetActive(false);
        delegateRespawn = GetRespawn(timeToRespawn);
        StartCoroutine(delegateRespawn);
    }

    IEnumerator GetRespawn(float time)
    {
        yield return new WaitForSeconds(time);

        Respawn();
    }

    public void Respawn()
    {
        Instantiate(spawnParticles, playerController.transform.position, Quaternion.identity);
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
        playerController.dashState.available = true;

        playerController.ChangeState(playerController.idleState);

        StartCoroutine(WaitFrameToActivate());

    }

    private IEnumerator WaitFrameToActivate()
    {
        yield return null;
        playerController.dashState.walkTrail.enabled = true;

    }

}

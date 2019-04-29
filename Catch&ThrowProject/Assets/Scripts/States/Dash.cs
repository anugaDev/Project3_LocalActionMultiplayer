using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : BaseState
{
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private float exitSpeedMultiplier = 0.10f;
    [SerializeField] private float imageShakeForce;
    [SerializeField] private float imageShakeTime;

    [SerializeField] public Collider playerTrigger;

    [SerializeField] private bool catched = false;

    public bool available = true;

    public float cooldown = 1.5f;

    private float timer;
    private GameUtilities gameUtils = new GameUtilities();

    [SerializeField] private Image cooldownVisual;
    [SerializeField] private ParticleSystem dashParticles;

    public override void Enter()
    {
        playerController.gameObject.layer = playerController.jumpLayer;
        available = false;

        playerTrigger.isTrigger = true;

        Vector3 direction = playerController.inputControl.RightDirection.normalized;
        playerController.rigidbody.velocity = (direction == Vector3.zero ? transform.right : direction) * speed;

        dashParticles.Play();
        StartCoroutine(StopDash());
    }

    private void Update()
    {
        if (available || playerController.stateMachine.currentState == this) return;

        timer += Time.deltaTime;
        cooldownVisual.fillAmount = timer / cooldown;

        if (timer >= cooldown)
        {
            StartCoroutine(gameUtils.ShakeObject(imageShakeTime,cooldownVisual.transform,imageShakeForce ));
            available = true;
            timer = 0;
        }
    }

    public override void Exit()
    {
        playerController.gameObject.layer = playerController.normalLayer;
        playerTrigger.isTrigger = catched ? true : false;

        dashParticles.Stop();

        catched = false;
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(duration);

        playerController.ChangeState(playerController.idleState);
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerController.stateMachine.currentState != playerController.dashState) return;

        var enemy = other.GetComponentInParent<PlayerController>();

        if (enemy && enemy.shield.shieldDestroyed) CatchPlayer(enemy);
    }

    private void CatchPlayer(PlayerController enemy)
    {
        RepositionEnemy(enemy);

        StopAllCoroutines();

        catched = true;

        enemy.ChangeState(enemy.caughtState);

        playerController.caughtPlayer = enemy;
        playerController.ChangeState(playerController.catchState);
    }

    private void RepositionEnemy(PlayerController enemy)
    {
        Vector3 directionBetweenPlayers = (transform.position - enemy.transform.position).normalized;

        enemy.transform.position = transform.position + (directionBetweenPlayers * .5f);
    }
}

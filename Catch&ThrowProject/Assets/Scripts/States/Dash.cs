using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BaseState
{
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private float exitSpeedMultiplier = 0.10f;

    [SerializeField] public Collider playerTrigger;

    [SerializeField] private bool catched = false;

    public bool available = true;

    public float cooldown = 1.5f;

    public override void Enter()
    {
        available = false;

        playerTrigger.isTrigger = true;
        playerController.rigidbody.velocity = playerController.inputControl.Direction * speed;

        StartCoroutine(StopDash());
    }

    private void Update()
    {

    }

    public override void Exit()
    {
        playerController.rigidbody.velocity *= exitSpeedMultiplier;
        playerTrigger.isTrigger = catched ? true : false;

        catched = false;

        StartCoroutine(Cooldown());
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(duration);

        playerController.ChangeState(playerController.idleState);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);

        available = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerController.stateMachine.currentState != playerController.dashState) return;

        var enemy = other.GetComponentInParent<PlayerController>();

        if (enemy) CatchPlayer(enemy);
    }

    private void CatchPlayer(PlayerController enemy)
    {
        StopAllCoroutines();
        catched = true;

        enemy.ChangeState(enemy.caughtState);

        playerController.caughtPlayer = enemy;
        playerController.ChangeState(playerController.catchState);
    }
}

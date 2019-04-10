using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BaseState
{
    [SerializeField] private readonly float speed;
    [SerializeField] private readonly float duration;

    [SerializeField] private Collider playerTrigger;

    private bool catched = false;

    public override void Enter()
    {
        //TODO: Animation, rotation and particles

        playerTrigger.isTrigger = true;
        playerController.rigidbody.velocity = playerController.inputControl.Direction * speed;

        StartCoroutine(StopDash());
    }

    public override void Exit()
    {
        //TODO: Stop animation
        playerController.rigidbody.velocity = Vector3.zero;
        playerTrigger.isTrigger = !catched;
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(duration);

        playerController.ChangeState(playerController.idleState);
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<PlayerController>();

        if (enemy && !enemy.Invulnerable) CatchPlayer(enemy);
    }

    private void CatchPlayer(PlayerController enemy)
    {
        StopCoroutine(StopDash());

        catched = true;

        enemy.ChangeState(enemy.caughtState);

        playerController.caughtPlayer = enemy;
        playerController.ChangeState(playerController.catchState);
    }
}

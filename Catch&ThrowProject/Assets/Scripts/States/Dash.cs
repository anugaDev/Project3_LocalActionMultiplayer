using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BaseState
{
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private float exitSpeedMultiplier = 0.10f;

    [SerializeField] private Collider playerTrigger;

    private bool catched = false;

    public override void Enter()
    {
        //TODO: Animation, rotation and particles

        playerTrigger.isTrigger = true;
        print(playerController.rigidbody.velocity);
        playerController.rigidbody.velocity = playerController.inputControl.Direction * speed;
        print(playerController.rigidbody.velocity);

        StartCoroutine(StopDash());
    }

    public override void Exit()
    {
        //TODO: Stop animation
        playerController.rigidbody.velocity *= exitSpeedMultiplier;
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

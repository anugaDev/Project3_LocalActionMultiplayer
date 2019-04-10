using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BaseState
{
    public float speed;
    public float duration;

    public Collider playerTrigger;

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

        if (catched)
        {
            catched = false;

        }
        else
        {
            playerTrigger.isTrigger = false;

        }
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(duration);

        playerController.ChangeState(playerController.idleState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            catched = true;

            StopCoroutine(StopDash());

            //ChangeState to grab
        }
    }
}

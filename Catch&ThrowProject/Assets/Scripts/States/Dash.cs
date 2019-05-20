using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : BaseState
{
    [SerializeField] private float speed;
    [SerializeField] private float pushBackForce;
    [SerializeField] private float duration;
    [SerializeField] private float exitSpeedMultiplier = 0.10f;
    [SerializeField] private float imageShakeForce;
    [SerializeField] private float imageShakeTime;

    [SerializeField] private float verticalSpeedDecayMultiplier = 0.75f;

    [SerializeField] public Collider playerTrigger;

    [SerializeField] private bool catched = false;

    public bool available = true;
    public bool released = true;

    public float cooldown = 1.5f;

    private float timer = 0;
    private GameUtilities gameUtils = new GameUtilities();
    private Vector3 actualDirection;

    [SerializeField] private ParticleSystem dashParticles;

    public override void Enter()
    {
        Vector3 direction = playerController.inputControl.Direction;
        
        if(direction.y > 0) playerController.gameObject.layer = playerController.jumpLayer;
        available = false;

        playerTrigger.isTrigger = true;

       
        playerController.rigidbody.velocity = (direction == Vector3.zero ? transform.right : direction) * speed;
        actualDirection = direction;

        dashParticles.Play();
        StartCoroutine(StopDash());
    }

    private void Update()
    {
        if (available || playerController.stateMachine.currentState == this) return;

        if(released)timer += Time.deltaTime;
        playerController.uiPanel.UpdateDashFill(timer,cooldown);

        if (timer >= cooldown)
        {
//            StartCoroutine(gameUtils.ShakeObject(imageShakeTime, cooldownVisual.transform, imageShakeForce));
            available = true;
            timer = 0;
        }
    }

    

    public override void Exit()
    {
        playerController.rigidbody.velocity = new Vector3 (playerController.onGround ? 0 : playerController.rigidbody.velocity.x,
                                                          playerController.rigidbody.velocity.y * verticalSpeedDecayMultiplier,
                                                          0);

        
        if(!catched) playerController.gameObject.layer = playerController.normalLayer;
        playerTrigger.isTrigger = catched ? true : false;

        dashParticles.Stop();

        catched = false;
    }
    public void InstantDashReload()
    {
        timer = cooldown;
        available = true;
        playerController.uiPanel.UpdateDashFill(timer,cooldown);


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

        if (!enemy) return;


        if (enemy.shield.shieldDestroyed)
        {
            CatchPlayer(enemy);
            return;
        }
//        else
//        {
//            playerController.stunState.stunByTime = true;
//
//            playerController.ChangeState(playerController.stunState);
//            playerController.Impulse(actualDirection,pushBackForce, true);
//
//        }
//
//        if (enemy.stateMachine.currentState == enemy.dashState)
//        {
//            enemy.stunState.stunByTime = true;
//
//            enemy.ChangeState(enemy.stunState);
//            enemy.Impulse(-actualDirection,pushBackForce, true);
//
//        }
    }

    private void CatchPlayer(PlayerController enemy)
    {
        RepositionEnemy(enemy);

        StopAllCoroutines();

        catched = true;
        enemy.gameObject.layer = enemy.jumpLayer;


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

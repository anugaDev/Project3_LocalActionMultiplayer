﻿using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class Dash : BaseState
{
    [SerializeField] private float speed;
    [SerializeField] private float pushBackForce;
    [SerializeField] private float duration;
    

    [SerializeField] private float verticalSpeedDecayMultiplier = 0.75f;

    [SerializeField] public Collider playerTrigger;

    [SerializeField] private bool catched = false;

    [SerializeField] private float timeAfterDashThreshold;
    private float lastDashTime;

    public bool available = true;
    public bool released = true;

    public float cooldown = 1.5f;

    private float timer = 0;
    private Vector3 actualDirection;

    public ParticleSystem dashParticles;

    [HideInInspector] public ParticleSystem.MainModule main;
    public TrailRenderer walkTrail;
    
    [FMODUnity.EventRef] public string onCooldown;
    [FMODUnity.EventRef] public string cooldownCompleted;
    [FMODUnity.EventRef] public string clashSound;

    private void Awake()
    {
        main = dashParticles.main;
    }


    private IEnumerator stopDash;

    public override void Enter()
    {
        Vector3 direction = playerController.inputControl.Direction;

        if (playerController.inputControl.keyboardAndMouse)
            direction = playerController.inputControl.PlayerToMouseDirection();
        if (direction.y > 0) playerController.gameObject.layer = playerController.jumpLayer;
        available = false;

        playerTrigger.isTrigger = true;

        lastDashTime = Time.timeSinceLevelLoad;

        playerController.Impulse(direction == Vector3.zero ? transform.right : direction, speed, false);
        actualDirection = direction;

        playerController.OrientatePlayer(actualDirection.x);

        dashParticles.Play();

        stopDash = StopDash();
        StartCoroutine(stopDash);


        base.Enter();

    }

    private void Update()
    {
        if (available || playerController.stateMachine.currentState == this) return;

        if (released) timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            available = true;
            RuntimeManager.PlayOneShot(cooldownCompleted);
            walkTrail.enabled = true;
            timer = 0;
        }
    }

    public override void Exit()
    {
        walkTrail.enabled = false;

        StopCoroutine(stopDash);

        playerController.rigidbody.velocity = new Vector3(playerController.CheckForGround() ? 0 : playerController.rigidbody.velocity.x,
                                                          playerController.rigidbody.velocity.y * verticalSpeedDecayMultiplier,
                                                          0);

        if (!catched) playerController.gameObject.layer = playerController.normalLayer;
        playerTrigger.isTrigger = catched ? true : false;

        dashParticles.Stop();

        catched = false;

        base.Exit();
    }

    public void InstantDashReload()
    {
        timer = cooldown;
        available = true;
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

        if (enemy.Invulnerable) return;

        if (enemy.shield.shieldDestroyed)
        {
            if (playerController.shield.shieldDestroyed && enemy.stateMachine.currentState == enemy.dashState || enemy.dashState.InsideMashTime())
            {
                MashWithPlayer(enemy);
            }
            else
                CatchPlayer(enemy);


        }
        else
        {
            var collideDirection = actualDirection;
        
            playerController.ChangeState(playerController.stunState);
            playerController.rigidbody.velocity = Vector3.zero;
            var groundDir = Vector3.up;
            groundDir.x = (-collideDirection.x);
            playerController.Impulse(playerController.CheckForGround() ? groundDir : -collideDirection, pushBackForce, true);

            if (enemy.stateMachine.currentState != enemy.dashState) return;

            enemy.ChangeState(enemy.stunState);
            enemy.Impulse(collideDirection, pushBackForce, true);

        }
    }

    private void CatchPlayer(PlayerController enemy)
    {
        RepositionEnemy(enemy);

        StopAllCoroutines();

        catched = true;
        enemy.gameObject.layer = enemy.jumpLayer;


        enemy.ChangeState(enemy.caughtState);

        playerController.Invulnerable = true;
        enemy.Invulnerable = true;

        playerController.caughtPlayer = enemy;
        playerController.ChangeState(playerController.catchState);


    }

    private void MashWithPlayer(PlayerController enemy)
    {

        RepositionEnemy(enemy);
        StopAllCoroutines();

        RuntimeManager.PlayOneShot(clashSound);
        playerController.caughtPlayer = enemy;
        enemy.caughtPlayer = playerController;

        playerController.Invulnerable = true;
        enemy.Invulnerable = true;

        playerController.ChangeState(playerController.mashButtonState);
        enemy.ChangeState(enemy.mashButtonState);

    }

    private void RepositionEnemy(PlayerController enemy)
    {
        Vector3 directionBetweenPlayers = (transform.position - enemy.transform.position).normalized;

        enemy.transform.position = transform.position + (directionBetweenPlayers * .5f);
    }

    public void PlayCooldownSound()
    {
        RuntimeManager.PlayOneShot(onCooldown);
    }

    public bool InsideMashTime()
    {
        return (Time.timeSinceLevelLoad - lastDashTime) <= timeAfterDashThreshold;
    }
}

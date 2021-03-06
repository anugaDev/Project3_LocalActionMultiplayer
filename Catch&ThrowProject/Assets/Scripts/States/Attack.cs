﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseState
{
    [SerializeField] private GameObject projectile;

    [Header("Shoot: ")]

    [SerializeField] private float timeAfterMelee;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float shootOffset;
    [SerializeField] private float shootRecoilForce;

    [Header("Direction: ")]

    public Transform directionAffordance;
    [SerializeField] private float directionAffordanceDistance;

    private Vector3 lastDir;

    [Header("Melee: ")]
    [SerializeField] private GameObject meleeParticles;

    [SerializeField] private float meleeDetectionAngle;
    [SerializeField] private float meleeDetectionDistance;
    [SerializeField] private LayerMask meleeDetectionMask;
    [SerializeField] private float meleeHitDamage;
    [SerializeField] private float hitForce;

    [SerializeField] private Color playerColor;

    [Header(("Attack Sound"))]
    
    [FMODUnity.EventRef] public string meleeSound;
    private FMOD.Studio.EventInstance meleeEventSound;
    [FMODUnity.EventRef] public string throwSound;
    private FMOD.Studio.EventInstance throwSoundEvent;

    private void Start()
    {
        meleeEventSound = FMODUnity.RuntimeManager.CreateInstance(meleeSound);
        throwSoundEvent = FMODUnity.RuntimeManager.CreateInstance(throwSound);

    }

    public override void Enter()
    {
        base.Enter();
        playerController.animator.speed = 0;
        directionAffordance.gameObject.SetActive(true);
        lastDir = transform.right.normalized;

        playerController.ammo.enabled = true;
        playerController.ammoBG.enabled = true;
        playerController.ammo.text = playerController.actualAmmo.ToString();
        playerController.ammoBG.text = playerController.actualAmmo.ToString();

    }

    public override void Execute()
    {

        #region UpdateMovement
        if (!playerController.CheckForGround())
        {
            playerController.fallState.CheckForCrossingPlatforms();
            playerController.fallState.ExecuteFallSpeed();

        }
        else
        {
            playerController.animator.SetBool(playerController.fallState.groundedBool, true);
            playerController.animator.SetBool(playerController.fallState.animationFallingBool, false);

            var velocity = playerController.rigidbody.velocity;
            velocity.x = 0;
            playerController.rigidbody.velocity = velocity;
        }
        #endregion


        #region Direction Affordance

        var actualDirection = (Vector3)playerController.inputControl.Direction;
        var direction = lastDir;
        if (actualDirection != Vector3.zero)
        {
            direction = actualDirection;
            lastDir = actualDirection;
        }
        

        directionAffordance.position = transform.position + direction * directionAffordanceDistance;

        var rotationAffordanceZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotationAffordance = Quaternion.Euler(0, 0, rotationAffordanceZ);
        directionAffordance.rotation = rotationAffordance;

        float animationDirection = Vector3.Dot(transform.up, (directionAffordance.position - transform.position).normalized);
        playerController.animator.SetFloat("ThrowDirection", animationDirection);
        
        playerController.OrientatePlayer(direction.x);



        #endregion


        #region Attack
        if (playerController.inputControl.ButtonIsUp(InputController.Button.FIRE))
        {
            if (direction == Vector3.zero) direction = transform.right.normalized;
            PlayerController enemyPlayer = IsMelee(playerController.inputControl.Direction);


            if (enemyPlayer) HitMelee(enemyPlayer, direction);
            else
                if (playerController.PlayerHasAmmo()) ShootProjectile(direction);
            else ActionWithoutAmmo();
        }
        #endregion
    }

    private PlayerController IsMelee(Vector3 direction)
    {
        var detected = Physics.OverlapSphere(transform.position, meleeDetectionDistance, meleeDetectionMask);

        PlayerController playerReturn = null;

        foreach (var playerTransform in (detected))
        {
            var controller = playerTransform.gameObject.GetComponent<PlayerController>();
            if (controller == null) continue;
            if (controller == playerController) continue;
            if (!(Vector3.Angle((controller.transform.position - transform.position).normalized, direction) < meleeDetectionAngle)) continue;


            if (playerReturn == null) playerReturn = controller;
            else if ((controller.transform.position - transform.position).magnitude <
                     (playerReturn.transform.position - transform.position).magnitude)
                playerReturn = controller;
        }

        return playerReturn;

    }

    public override void Exit()
    {
        playerController.animator.speed = 1;

        playerController.ammo.enabled = false;
        playerController.ammoBG.enabled = false;

        directionAffordance.gameObject.SetActive(false);
        
        StopAllCoroutines();
    }

    public void ShootProjectile(Vector2 direction)
    {
        var speed = projectileSpeed;

        var projectileInstance = Instantiate(projectile, transform.position + (shootOffset * (Vector3)direction), Quaternion.identity);
        var projectileClass = projectileInstance.GetComponent<Projectile>();
        projectileClass.SetBullet(direction, speed, playerController, playerController.playerSkin == null ? Color.red : playerController.playerSkin.mainColor);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), projectileInstance.GetComponent<Collider>(), true);

        var force = Vector3.up * shootRecoilForce;
        if (Mathf.Abs(force.magnitude) > 0 && direction.y <= 0 && !playerController.onGround) playerController.rigidbody.velocity = force;

        playerController.ConsumeAmmo(1);

        AttackFinished();

        playerController.animator.speed = 1;

        throwSoundEvent.start();
    }

    public void HitMelee(PlayerController enemyPlayer, Vector3 direction)
    {
        Instantiate(meleeParticles, enemyPlayer.transform.position, transform.rotation);
        enemyPlayer.MeleeHit(meleeHitDamage, direction, hitForce, playerController);

        meleeEventSound.start();

        StartCoroutine(WaitAttackToEnd(timeAfterMelee));
    }

    private IEnumerator WaitAttackToEnd(float time)
    {
        yield return new WaitForSeconds(time);
        
        AttackFinished();
        
    }

    private void ActionWithoutAmmo()
    {
        AttackFinished();
    }

    public void AttackFinished()
    {
        playerController.ChangeState(playerController.idleState);
    }

   
}
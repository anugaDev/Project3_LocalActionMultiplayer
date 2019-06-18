using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowParticles : MonoBehaviour
{
    public ParticleSystem particles;
    public PlayerController player;

    void Start()
    {
        particles.startColor = player.playerSkin.mainColor;
    }

    void Update()
    {
        player.dashState.walkTrail.enabled = false;
        transform.parent.up = -player.rigidbody.velocity.normalized;
        transform.localRotation = Quaternion.Euler(0, 90, 90);

        if (player.onGround ||
            player.stateMachine.currentState == player.jumpState ||
            player.stateMachine.currentState == player.doubleJumpState ||
            player.stateMachine.currentState == player.dashState)
        {
            player.dashState.walkTrail.enabled = true;

            gameObject.SetActive(false);
        }
    }
}

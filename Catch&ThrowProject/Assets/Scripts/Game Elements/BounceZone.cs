using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class BounceZone : MonoBehaviour
{
    [SerializeField] private float bounceForce;
    [FMODUnity.EventRef] public string bounceSound;


    public void BounceObject(Rigidbody rigidbody, PlayerController player)
    {
        if (rigidbody.velocity.y >= bounceForce) return;
        
        var velocity = rigidbody.velocity;
        velocity.y = bounceForce;
        rigidbody.velocity = velocity;

        RuntimeManager.PlayOneShot(bounceSound);

        if (player != null)
        {
            player.jumpState.commingFromJump = false;
            player.jumpMade = false;
        }
    }
}

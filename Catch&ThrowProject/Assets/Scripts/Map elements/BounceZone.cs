using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceZone : MonoBehaviour
{
    [SerializeField] private float bounceForce;

    public void BounceObject(Rigidbody rigidbody,PlayerController player)
    {
        var velocity = rigidbody.velocity;
        velocity.y = bounceForce;
        rigidbody.velocity = velocity;
        if (player != null)
            player.jumpState.commingFromJump = false;
    }
}

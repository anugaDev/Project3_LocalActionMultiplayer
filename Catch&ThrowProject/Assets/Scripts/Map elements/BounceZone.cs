using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceZone : MonoBehaviour
{
    [SerializeField] private float bounceForce;

    public void BounceObject(Rigidbody rigidbody)
    {
        var velocity = rigidbody.velocity;
        velocity.y = bounceForce;
        rigidbody.velocity = velocity;
    }
}

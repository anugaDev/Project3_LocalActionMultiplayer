using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField]private Rigidbody rigidbody;
    private Vector2 direction;
    public float damage;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rigidbody.velocity = direction * projectileSpeed;
    }

    
    public void SetBullet(Vector2 newDirection, float speed)
    {
        direction = newDirection;
        projectileSpeed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
//        var tag = other.transform.tag;

        Destroy(gameObject);
    }

 
}

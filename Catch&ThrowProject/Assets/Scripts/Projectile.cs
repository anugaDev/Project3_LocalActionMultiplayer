using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField]  private float damage;
    [SerializeField]  private float hitForce;
    private Vector2 direction;

    private bool nailed = false;

    private void FixedUpdate()
    {
        if (!nailed) rigidbody.velocity = direction * projectileSpeed * Time.fixedDeltaTime;
    }
    public void SetBullet(Vector2 newDirection, float speed)
    {
        direction = newDirection;
        projectileSpeed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();

            if (!nailed) player.ProjectileHit(direction,hitForce,damage);
            else
            {
                player.ResupplyAmmo(1);
            }

            Destroy(gameObject);
            return;
        }
        
        else if(other.gameObject.CompareTag("Death Zone")) Destroy(gameObject);
        
        else if(other.gameObject.CompareTag("Cross Zone")) other.gameObject.GetComponent<CrossZone>().ObjectCross(transform);

        rigidbody.isKinematic = true;
        
        rigidbody.velocity = UnityEngine.Vector3.zero;
        nailed = true;
    }

    private void OnDestroy()
    {
        //Game manager erase.
    }
}

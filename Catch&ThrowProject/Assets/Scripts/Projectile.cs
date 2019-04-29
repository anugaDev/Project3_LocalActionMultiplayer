using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Rigidbody rigidbody;
    private Vector2 direction;
    public float damage;
    public bool nailed = false;

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

            if (!nailed) player.ProjectileHit(this);
            else player.ResupplyAmmo(1f);

            Destroy(gameObject);
            return;
        }

        rigidbody.velocity = UnityEngine.Vector3.zero;
        nailed = true;
    }
}

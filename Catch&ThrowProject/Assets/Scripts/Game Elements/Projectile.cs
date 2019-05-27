using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float damage;
    [SerializeField] private float hitForce;
    
    [SerializeField] private Collider impactCollider;
    [SerializeField] private float detectionRadius;
    
    [SerializeField] private PlayerController originPlayer;
    [SerializeField] private float timeBeforeAutoDestroy;
    [SerializeField] private float timeToIgnorePlayer = 0.2f;
    [SerializeField] private Animation animation;
    [SerializeField] private Outline projectileOutline;

    [SerializeField] private Color neutralTakeColor = Color.white;

    [SerializeField]private bool nailed = false;

    [SerializeField] private float projectileDownThreshold = -0.1f;
    [SerializeField] private int UpLayer;
    [SerializeField] private int downLayer;
    
    private UnityEngine.Vector3 direction;
    private bool offPlayerZone = false;
    
    private void Update()
    {
        if (!nailed)
        {
            rigidbody.velocity = direction * projectileSpeed * Time.deltaTime;
            
            if(offPlayerZone)
                Physics.IgnoreCollision(impactCollider, originPlayer.normalCollider,originPlayer.AmmoIsMax());

        }
        
    }
    public void SetBullet( UnityEngine.Vector3  newDirection, float speed, PlayerController originplayer, Color playerColor)
    {
        StartCoroutine(StopIgnoringAfterTime());
        StartCoroutine(DestroyAfterTime());
        direction = newDirection;
        projectileSpeed = speed;
        originPlayer = originplayer;
        projectileOutline.OutlineColor = playerColor;

        if (direction.y >= projectileDownThreshold) gameObject.layer = UpLayer;
        else gameObject.layer = downLayer;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Player");
            var player = other.gameObject.GetComponent<PlayerController>();


            if (!nailed)
            {
                if (player == originPlayer)
                {
                    player.ResupplyAmmo(1);
                }
                else
                {
                    player.ProjectileHit(direction,hitForce,damage);
                }
            }
            else
            {
                if (player.AmmoIsMax()) return;
                player.ResupplyAmmo(1);

            }
            
            Destroy(gameObject);
            
        }
        
        else if(other.gameObject.CompareTag("Death Zone"))
        {
            var newDir =  UnityEngine.Vector3.Reflect(direction,other.transform.position.normalized);
            newDir.z = 0;
            direction = newDir;
        }
        
        else if(other.gameObject.CompareTag("Cross Zone")) other.gameObject.GetComponent<CrossZone>().ObjectCross(transform);


        else
        {
            print("nail");
            Nail(); 
        }

    }

    private void Nail()
    {
        projectileOutline.OutlineColor = neutralTakeColor;
        rigidbody.velocity = UnityEngine.Vector3.zero;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        nailed = true;
        animation.Stop();

        var index = 0;
        while (Physics.OverlapSphere(transform.position, detectionRadius,
            LayerMask.GetMask("Default")).Any())
        {
            transform.position -= direction;
        }
        Physics.IgnoreCollision(impactCollider, originPlayer.normalCollider,false);

    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(timeBeforeAutoDestroy);
        
        Destroy(gameObject);
    }
    private IEnumerator StopIgnoringAfterTime()
    {
        yield return new WaitForSeconds(timeToIgnorePlayer);

        offPlayerZone = true;

    }

    private void OnDestroy()
    {
        
    }
}

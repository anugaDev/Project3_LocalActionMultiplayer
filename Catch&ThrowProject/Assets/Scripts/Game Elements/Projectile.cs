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
    [SerializeField] private TrailRenderer trail;

    [SerializeField] private Color neutralTakeColor = Color.white;

    [SerializeField]private bool nailed = false;

    [SerializeField] private float projectileDownThreshold = -0.1f;
    [SerializeField] private int UpLayer;
    [SerializeField] private int downLayer;

    private bool offPlayerZone = false;
    
    private UnityEngine.Vector3 direction;
    
    private void Update()
    {
        if (!nailed)
        {
            rigidbody.velocity = direction * projectileSpeed * Time.deltaTime;
            
            if(offPlayerZone)
                if(!nailed)
                    Physics.IgnoreCollision(impactCollider, originPlayer.normalCollider,originPlayer.AmmoIsMax());

        }
        
    }
    public void SetBullet( UnityEngine.Vector3  newDirection, float speed, PlayerController originplayer, Color playerColor)
    {
        StartCoroutine(DestroyAfterTime());
        StartCoroutine(WaitForExitOriginZone());
        
        direction = newDirection;
        projectileSpeed = speed;
        originPlayer = originplayer;
        trail.material.color = playerColor;

        if (direction.y >= projectileDownThreshold) gameObject.layer = UpLayer;
        else gameObject.layer = downLayer;
        
        _LevelManager.instance.scatteredAmmo += 1;

    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<PlayerController>();

            if (!nailed)
            {
                if (player == originPlayer)
                {
                    if(offPlayerZone && !player.AmmoIsMax())
                        player.ResupplyAmmo(1);
                }
                else
                {
                    player.ProjectileHit(direction,hitForce,damage);
                }
            }
            else
            {
//                if (player.AmmoIsMax()) return;
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
            Nail(); 
        }

    }

    private void Nail()
    {
        trail.enabled = false;
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
    private IEnumerator WaitForExitOriginZone()
    {
        yield return new WaitForSeconds(timeToIgnorePlayer);

        offPlayerZone = true;

    }


    private void OnDestroy()
    {
       _LevelManager.instance.scatteredAmmo -= 1;
    }
}

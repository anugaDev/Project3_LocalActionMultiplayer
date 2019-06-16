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

    private bool nailed = false;

    [SerializeField] private float projectileDownThreshold = -0.1f;
    [SerializeField] private int UpLayer;
    [SerializeField] private int downLayer;

    [SerializeField] private GameObject impactParticles;

    [Header("Nailed Detection")] [SerializeField]
    private LayerMask nailedLayerMask;

    [SerializeField] private float nailedDistance;
    
    [Header("sound")]
    
    [FMODUnity.EventRef] public string thrownSound;
    private FMOD.Studio.EventInstance thrownEventSound;
    [FMODUnity.EventRef] public string wallHitSound;
    [FMODUnity.EventRef] public string bounceSound;
    [FMODUnity.EventRef] public string playerHitSound;
    

    private bool offPlayerZone = false;
    
    private UnityEngine.Vector3 direction;

    private void Awake()
    {
        thrownEventSound = FMODUnity.RuntimeManager.CreateInstance(thrownSound);


    }

    private void Update()
    
    {
        if (!nailed)
        {
            rigidbody.velocity = direction * projectileSpeed;
            
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

        thrownEventSound.start();

    }

    
    private void OnTriggerEnter(Collider other)
    {
        thrownEventSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        if (other.gameObject.CompareTag("DestroyProjectiles"))
        {
            Destroy(gameObject);

        }
        
        else if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<PlayerController>();

            if (!nailed)
            {
                if (player == originPlayer)
                {
                    if (offPlayerZone && !player.AmmoIsMax())
                    {
                        player.ResupplyAmmo(1);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    FMODUnity.RuntimeManager.PlayOneShot(playerHitSound);
                    player.ProjectileHit(direction,hitForce,damage);
                    Instantiate(impactParticles, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
            else
            {
                if (player.AmmoIsMax()) return;

                if (!IsDirectionObstructed(player, player.transform.position - transform.position,
                    nailedDistance))
                {
                    player.ResupplyAmmo(1);
                    Destroy(gameObject);

                }

            }
            
            
        }
        
        else if(other.gameObject.CompareTag("Death Zone"))
        {
            Instantiate(impactParticles, transform.position, Quaternion.identity);

            FMODUnity.RuntimeManager.PlayOneShot(bounceSound);
            var newDir =  UnityEngine.Vector3.Reflect(direction,other.transform.position.normalized);
            newDir.z = 0;
            direction = newDir;
        }
        
        else if(other.gameObject.CompareTag("Cross Zone")) other.gameObject.GetComponent<CrossZone>().ObjectCross(transform);


        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(wallHitSound);
            Nail(other.GetComponent<Collider>());
            Instantiate(impactParticles, transform.position, Quaternion.identity);

        }


    }

    private void Nail(Collider col)
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
        transform.position = col.ClosestPointOnBounds(transform.position) - (direction * detectionRadius);
        
        
        Physics.IgnoreCollision(impactCollider, originPlayer.normalCollider,false);


    }
    private bool IsDirectionObstructed(PlayerController player, UnityEngine.Vector3 direction, float distance)
    {
        if (player.gameObject.layer == player.jumpLayer) return false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction , out hit, distance, nailedLayerMask))
        {
            if (player.transform != hit.transform)
                return true;
            
        }

        return false;
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

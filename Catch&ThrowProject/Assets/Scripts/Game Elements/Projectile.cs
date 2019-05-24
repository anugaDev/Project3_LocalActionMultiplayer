﻿using System.Collections;
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
    [SerializeField] private SphereCollider impactCollider;
    [SerializeField] private PlayerController originPlayer;
    [SerializeField] private float timeBeforeAutoDestroy;
    [SerializeField] private Animation animation;
    private UnityEngine.Vector3 direction;
    [SerializeField] private Outline projectileOutline;

    [SerializeField] private Color neutralTakeColor = Color.white;

    [SerializeField]private bool nailed = false;

    [SerializeField] private float projectileDownThreshold = -0.1f;
    [SerializeField] private int UpLayer;
    [SerializeField] private int downLayer;
    
    private void Update()
    {
        if (!nailed) rigidbody.velocity = direction * projectileSpeed * Time.deltaTime;
    }
    public void SetBullet( UnityEngine.Vector3  newDirection, float speed, PlayerController originplayer, Color playerColor)
    {
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
            var player = other.GetComponent<PlayerController>();

            
            if (!nailed) player.ProjectileHit(direction,hitForce,damage);
            else
            {
                if (player.AmmoIsMax()) return;
                player.ResupplyAmmo(1);

            }
            Destroy(gameObject);

        }
        
        else if(other.gameObject.CompareTag("Death Zone")) Destroy(gameObject);
        
        else if(other.gameObject.CompareTag("Cross Zone")) other.gameObject.GetComponent<CrossZone>().ObjectCross(transform);
        
        Physics.IgnoreCollision(impactCollider, originPlayer.normalCollider,false);

        Nail();
    }

    private void Nail()
    {
        projectileOutline.OutlineColor = neutralTakeColor;
        rigidbody.velocity = UnityEngine.Vector3.zero;
        nailed = true;
        animation.Stop();

        var index = 0;
        while (Physics.OverlapSphere(transform.position, impactCollider.radius,
            LayerMask.GetMask("Default")).Any())
        {
            transform.position -= direction;
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(timeBeforeAutoDestroy);
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float defaultHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float regenerationSpeed;
    [SerializeField] private float regenerationDelay;
    [SerializeField] private float shakePortraitForce;
    [SerializeField] private float shakePortraitTime;
    [SerializeField] private Outline playerShieldOutline;
    [SerializeField] private Color lowShieldColor;
    [SerializeField] private Color shieldStableColor;
    [HideInInspector] public bool shieldDestroyed;
    [SerializeField] private Transform playerPortrait;
    private float actualHealth;

    private GameUtilities gameUtilities = new GameUtilities();
    

    private void Start()
    {       
        ResetShield();
    }
    private void Update()
    {       
        actualHealth = Mathf.Clamp(actualHealth, 0,defaultHealth);
        actualHealth += regenerationSpeed * Time.deltaTime;
        
        if (shieldDestroyed && actualHealth > lowHealthThreshold) 
            ShieldRegenerate();

        if (playerShieldOutline.enabled)
            playerShieldOutline.OutlineColor = actualHealth > lowHealthThreshold ? shieldStableColor : lowShieldColor;
    }

    public void Hit(float damage)
    {
        actualHealth -= damage;

        if (actualHealth <= 0)
        {
            DestroyShield();
        }
        else
            playerShieldOutline.OutlineColor = actualHealth < lowHealthThreshold ? lowShieldColor : shieldStableColor;
                   
    }

    public void ResetShield()
    {
        actualHealth = defaultHealth;
        playerShieldOutline.enabled = true;
        playerShieldOutline.OutlineColor = shieldStableColor;
        shieldDestroyed = false;
    }

    public void ShieldRegenerate()
    {
        shieldDestroyed = false;
        playerShieldOutline.enabled = true;
        playerShieldOutline.OutlineColor = lowShieldColor;
    }
  
    public void DestroyShield()
    {
        StartCoroutine(gameUtilities.ShakeObject(shakePortraitTime, playerPortrait, shakePortraitForce));
        actualHealth = 0;
        shieldDestroyed = true;
        var destroyShield = new Color(0, 0, 0);
        playerShieldOutline.enabled = false;
        playerShieldOutline.OutlineColor = destroyShield;
    }

   
   
}

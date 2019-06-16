using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float maxHealth;
    [SerializeField] private float defaultHealth;
    [SerializeField] private float lowHealthThreshold;

    [SerializeField] private float regenerationSpeed;
    [SerializeField] private float regenerationDelay;

    [SerializeField] private float shakePortraitForce;
    [SerializeField] private float shakePortraitTime;

    [SerializeField] private float blinkingTime;
    [SerializeField] private float timeBetweenBlinking;

    [SerializeField] private MeshRenderer shieldMesh;
    [SerializeField] private Transform playerModel;

    [SerializeField] private Color lowShieldColor;
    [SerializeField] private Color shieldStableColor;

    [HideInInspector] public bool shieldDestroyed;

    [SerializeField] private GameObject destroyShieldParticles;

    [Header("Sound")]
    [FMODUnity.EventRef] public string upShield;
    [FMODUnity.EventRef] public string downShield;
    
    
    
    private float actualHealth;

    public IEnumerator actualBlinking;

    private GameUtilities gameUtilities = new GameUtilities();
    private static readonly int MainColor = Shader.PropertyToID("_MainColor");

    private void Start()
    {
        ResetShield();
    }

    private void Update()
    {
        actualHealth = Mathf.Clamp(actualHealth, 0, maxHealth);
        actualHealth += regenerationSpeed * Time.deltaTime;

        if (shieldDestroyed && actualHealth > lowHealthThreshold) ShieldRegenerate();

        if (shieldMesh.enabled)
        {            
            var color = actualHealth > lowHealthThreshold ? shieldStableColor : lowShieldColor;
            shieldMesh.material.SetColor(MainColor, color);
        }
    }

    public void Hit(float damage)
    {
        actualHealth -= damage;

        if (actualHealth <= 0) DestroyShield();
        else
        {
            
            var color = actualHealth < lowHealthThreshold ? lowShieldColor : shieldStableColor;
            shieldMesh.material.SetColor(MainColor, color);
        }

        if (actualBlinking != null) StopCoroutine(actualBlinking);

        ImpactBlink();
    }

    public void ResetShield()
    {

        actualHealth = defaultHealth;
        shieldMesh.enabled = true;
        shieldMesh.material.SetColor(MainColor, shieldStableColor);
        shieldDestroyed = false;
    }

    public void ShieldRegenerate()
    {
        FMODUnity.RuntimeManager.PlayOneShot(upShield);
        shieldDestroyed = false;
        shieldMesh.enabled = true;
        shieldMesh.material.SetColor(MainColor,lowShieldColor);
    }

    public void ShieldResupply(float quantity)
    {
        actualHealth += quantity;
        actualHealth = Mathf.Clamp(actualHealth, 0, maxHealth);
    }

    public bool ShieldIsNotFull()
    {
        return actualHealth < maxHealth;
    }

    public void DestroyShield()
    {
        Instantiate(destroyShieldParticles, transform.position, Quaternion.identity);
        if(!shieldDestroyed) FMODUnity.RuntimeManager.PlayOneShot(downShield);

        if(actualBlinking != null) StopCoroutine(actualBlinking);
        
        actualBlinking = gameUtilities.Blink(playerModel, timeBetweenBlinking, blinkingTime);

        StartCoroutine(actualBlinking);
        
//        StartCoroutine(gameUtilities.ShakeObject(shakePortraitTime, playerController.uiPanel.transform, shakePortraitForce));

        actualHealth = 0;
        shieldDestroyed = true;

        shieldMesh.enabled = false;
    }

    public void ImpactBlink()
    {
        actualBlinking = gameUtilities.Blink(playerModel, timeBetweenBlinking, blinkingTime);

        StartCoroutine(actualBlinking);
    }
}

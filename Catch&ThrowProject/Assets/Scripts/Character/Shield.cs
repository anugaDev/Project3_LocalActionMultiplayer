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

    [SerializeField] private float blinkingTime;
    [SerializeField] private float timeBetweenBlinking;

    [SerializeField] private SpriteRenderer shieldSprite;
    [SerializeField] private Renderer objectRenderer;

    [SerializeField] private Color lowShieldColor;
    [SerializeField] private Color shieldStableColor;

    [SerializeField] private Transform playerPortrait;
    [HideInInspector] public bool shieldDestroyed;

    private float actualHealth;

    public IEnumerator actualBlinking;

    private GameUtilities gameUtilities = new GameUtilities();

    private void Start()
    {
        ResetShield();
    }

    private void Update()
    {
        actualHealth = Mathf.Clamp(actualHealth, 0, defaultHealth);
        actualHealth += regenerationSpeed * Time.deltaTime;

        if (shieldDestroyed && actualHealth > lowHealthThreshold) ShieldRegenerate();

        if (shieldSprite.enabled)
            shieldSprite.color = actualHealth > lowHealthThreshold ? shieldStableColor : lowShieldColor;
    }

    public void Hit(float damage)
    {
        actualHealth -= damage;

        if (actualHealth <= 0) DestroyShield();
        else shieldSprite.color = actualHealth < lowHealthThreshold ? lowShieldColor : shieldStableColor;

        if (actualBlinking != null) StopCoroutine(actualBlinking);

        ImpactBlink();
    }

    public void ResetShield()
    {
        
        actualHealth = defaultHealth;
        shieldSprite.enabled = true;
        shieldSprite.color = shieldStableColor;
        shieldDestroyed = false;
    }

    public void ShieldRegenerate()
    {
        shieldDestroyed = false;
        shieldSprite.enabled = true;
        shieldSprite.color = lowShieldColor;
    }

    public void DestroyShield()
    {
        StartCoroutine(gameUtilities.ShakeObject(shakePortraitTime, playerPortrait, shakePortraitForce));

        actualHealth = 0;
        shieldDestroyed = true;

        shieldSprite.enabled = false;
    }

    public void ImpactBlink()
    {
        actualBlinking = gameUtilities.Blink(objectRenderer, timeBetweenBlinking, blinkingTime);

        StartCoroutine(actualBlinking);
    }
}

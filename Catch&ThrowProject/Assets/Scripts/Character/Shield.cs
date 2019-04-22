using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float defaultHealth;
    [SerializeField] private float lowHealthThreshold;
    private float actualHealth;
    public bool shieldDestroyed;

    private void Start()
    {
        actualHealth = defaultHealth;
    }

    public void Hit(float damage)
    {
        actualHealth -= damage;
        UpdateShieldUI();
        
        if (actualHealth <= 0)
        {
            actualHealth = 0;
            if(!shieldDestroyed)
                DestroyShield();
            }
           
    }

    public void ResetShield()
    {
        actualHealth = defaultHealth;
        shieldDestroyed = false;
    }
  
    public void DestroyShield()
    {
        actualHealth = 0;
        shieldDestroyed = true;
    }

    private void UpdateShieldUI()
    {
        if (actualHealth <= 0 && !shieldDestroyed)
        {
            
        }
        else
        {
            
        }
    }
}

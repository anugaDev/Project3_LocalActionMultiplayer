using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float defaultHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float regenerationSpeed;
    private float actualHealth;
    public bool shieldDestroyed;
    
    

    private void Start()
    {
        actualHealth = defaultHealth;
    }

    public void Hit(float damage)
    {
        actualHealth -= damage;
        
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

    public void ShieldRegenerate()
    {
        shieldDestroyed = false;
    }
  
    public void DestroyShield()
    {
        actualHealth = 0;
        shieldDestroyed = true;
    }

   
    private void Update()
    {
        Mathf.Clamp(actualHealth, 0,regenerationSpeed);

         actualHealth += regenerationSpeed * Time.deltaTime;

        if (shieldDestroyed && actualHealth > lowHealthThreshold) 
            ShieldRegenerate();
    }
}

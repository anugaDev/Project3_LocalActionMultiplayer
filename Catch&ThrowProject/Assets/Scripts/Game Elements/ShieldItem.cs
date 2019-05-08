using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : MonoBehaviour
{
    [SerializeField] private float shieldResupplyQuantity;
    public void TakeShield(PlayerController player)
    {
        player.shield.ShieldResupply(shieldResupplyQuantity);
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var player= other.gameObject.GetComponent<PlayerController>();
        if(player.shield.ShieldIsNotFull())
            TakeShield(player);
    }
    private void OnDestroy()
    {
        
    }
}

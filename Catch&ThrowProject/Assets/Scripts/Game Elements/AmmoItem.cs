using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    [SerializeField] private int ammoResupply = 1;
    
    [FMODUnity.EventRef] public string pickUpSound;

    
    public void TakeAmmo(PlayerController player)
    {
        player.ResupplyAmmo(ammoResupply);
        RuntimeManager.PlayOneShot(pickUpSound);
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var player= other.gameObject.GetComponent<PlayerController>();

        if (player.AmmoIsMax()) return;
            TakeAmmo(player);
    }
    private void OnDestroy()
    {
        _LevelManager.instance.scatteredAmmo --;
    }
}

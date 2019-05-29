using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    [SerializeField] private int ammoResupply = 1;
    
    public void TakeAmmo(PlayerController player)
    {
        player.ResupplyAmmo(ammoResupply);
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        print("enter");
        if (!other.gameObject.CompareTag("Player")) return;
        var player= other.gameObject.GetComponent<PlayerController>();

        print("playerReload");
        if (player.AmmoIsMax()) return;
            TakeAmmo(player);
    }
    private void OnDestroy()
    {
        _LevelManager.instance.scatteredAmmo -= 1;
    }
}

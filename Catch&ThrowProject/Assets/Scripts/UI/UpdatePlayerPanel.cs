using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerPanel : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image shieldBar;
    [SerializeField] private Image DashFill;
    [SerializeField] private Image AmmoFill;

    [Header("Text")]
    [SerializeField] private Text ammo;
    [SerializeField] private Text remainingLives;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDashFill(float actual,float max)
    {
        UpdateFill(DashFill,actual,max);
    }
    
    public void UpdateShieldFill(float actual,float max)
    {
        UpdateFill(shieldBar,actual,max);
    }
    
    public void UpdateAmmoFill(float actual,float max)
    {
        UpdateFill(AmmoFill,actual,max);
    }

    public void SetAmmoFillActive(bool isActive)
    {
        AmmoFill.gameObject.SetActive(isActive);
    }

    public void UpdateAmmoText(float actualAmmo)
    {
        ammo.text = "Ammo : " + actualAmmo;
    }

    public void UpdateLivesText(float actualLives)
    {
        remainingLives.text = actualLives.ToString();
    }

    private void UpdateFill(Image fillImage, float actual, float max)
    {
        fillImage. fillAmount = actual / max;
    }
}

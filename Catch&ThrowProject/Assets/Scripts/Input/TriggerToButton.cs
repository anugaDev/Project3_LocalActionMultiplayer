using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToButton
{
    private float triggerDownThreshold;
    private string desiredButton;
    public bool triggerInUse;
   
    public void UpdateTrigger()
    {
        if (Input.GetAxis(desiredButton) > triggerDownThreshold) triggerInUse = !triggerInUse || triggerInUse;
        if (!(Input.GetAxis(desiredButton) < triggerDownThreshold)) return;
        triggerInUse = false;

    }
 
}

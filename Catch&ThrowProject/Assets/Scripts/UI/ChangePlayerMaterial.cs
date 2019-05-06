using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerMaterial : MonoBehaviour
{
    public PlayerSelectionPanel playerPanel;

    public List<Material> materialPresets;
    public int currentMaterial;

    public string SubmitButton;
    public string CancelButton;

    public string left;
    public string right;

    public enum InputDirection
    {
        Left,
        Right
    }

    private void Start()
    {
        currentMaterial = 0;
    }

    private void Update()
    {
        if (playerPanel.GameStarting) return;

        if (Input.GetButtonDown(SubmitButton + playerPanel.controllerNumber)) playerPanel.ReadyCheck(true);
        if (Input.GetButtonDown(CancelButton + playerPanel.controllerNumber)) playerPanel.ReadyCheck(false);

        if (playerPanel.readyPanel.gameObject.activeSelf) return;

        if (Input.GetButtonDown(left + playerPanel.controllerNumber)) playerPanel.ChangeColor(GetMaterial(InputDirection.Left));
        if (Input.GetButtonDown(right + playerPanel.controllerNumber)) playerPanel.ChangeColor(GetMaterial(InputDirection.Right));
    }

    private Material GetMaterial(InputDirection direction)
    {
        switch (direction)
        {
            case InputDirection.Left:
                if (currentMaterial == 0) currentMaterial = materialPresets.Count - 1;
                else currentMaterial--;
                break;
            case InputDirection.Right:
                if (currentMaterial == materialPresets.Count - 1) currentMaterial = 0;
                else currentMaterial++;
                break;
        }

        return materialPresets[currentMaterial];
    }
}

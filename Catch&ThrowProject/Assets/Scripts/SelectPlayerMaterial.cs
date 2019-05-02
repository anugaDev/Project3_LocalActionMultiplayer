using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerMaterial : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public int[] materialPositions;

    public GameObject panel;
    public Text readyText;

    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetButtonDown(GetComponent<PlayerSelectionPanel>().panelInput.cancelButton)) ReadyCheck(false);
    }

    public void ChangeColor(Material newMaterial)
    {
        var actualMaterials = meshRenderer.materials;

        for (int i = 0; i < materialPositions.Length; i++)
        {
            actualMaterials[materialPositions[0]] = newMaterial;
        }

        meshRenderer.materials = actualMaterials;
    }

    public void ReadyCheck(bool ready)
    {
        panel.SetActive(!ready);
        readyText.enabled = ready;
    }
}

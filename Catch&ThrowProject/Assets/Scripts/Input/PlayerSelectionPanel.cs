using System.Collections;
using System.Collections.Generic;
using Assets.Resources;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour
{
    public bool HasPlayer = false;
    public bool GameStarting = false;

    public int controllerNumber;
    public int playerNumber;

    public Image parentPanel;
    public Text pressToAssignText;
    public Text playerNumberText;

    public SkinnedMeshRenderer dummyMesh;
    public SkinnedMeshRenderer maskMesh;

    public List<Skin> availableSkins;
    public Skin playerSkin;

    public GameObject buttonPanel;
    public GameObject readyPanel;

    public void Recolor(int index)
    {
        playerSkin = availableSkins[index];

        dummyMesh.material.mainTexture = playerSkin.playerTexture;
        maskMesh.material.mainTexture = playerSkin.maskTexture;
    }

    public void AssignController(int controllerNumber)
    {
        playerNumberText.text = "P" + playerNumber;
        parentPanel.enabled = false;
        pressToAssignText.enabled = false;

        HasPlayer = true;
        this.controllerNumber = controllerNumber;

        gameObject.SetActive(true);
    }

    public void ReadyCheck(bool ready)
    {
        if (!ready && !readyPanel.activeSelf) return;

        if (playerSkin.used && ready)
        {
            print("Skin already chosen");
            return;
        }

        playerSkin.used = ready;
        buttonPanel.SetActive(!ready);
        readyPanel.SetActive(ready);
        readyPanel.GetComponent<Image>().color = playerSkin.mainColor;
    }
}

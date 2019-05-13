using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _GameManager : MonoBehaviour
{
    public static _GameManager instance;

    public List<PlayerSelectionPanel> players;
    public GameObject playerBasicPrefab;

    public int SceneToLoadNumber = -1;
    public int lastScene;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);

        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += StartGame;
    }

    public void LoadNewGame()
    {
        lastScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(SceneToLoadNumber);

        foreach (PlayerSelectionPanel player in players) { player.GameStarting = true; }
    }

    private void CreatePlayer(PlayerSelectionPanel playerInfo)
    {
        GameObject player = Instantiate(playerBasicPrefab);

        player.GetComponent<InputController>().controllerNumber = playerInfo.controllerNumber;
        player.GetComponent<PlayerController>().playerMesh.material = playerInfo.dummyMat;
        player.GetComponent<PlayerController>().maskMesh.material = playerInfo.maskMat;
        player.GetComponent<PlayerController>().enabled = false;

        _LevelManager.instance.players.Add(player.GetComponent<PlayerController>());
    }

    public void StartGame(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != SceneToLoadNumber) return;

        foreach (PlayerSelectionPanel player in players) { CreatePlayer(player); }

        _LevelManager.instance.SetNewGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

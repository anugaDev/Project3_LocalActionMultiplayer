using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _GameManager : MonoBehaviour
{
    public static _GameManager instance;

    public List<PlayerSelectionPanel> players;
    public List<PlayerController> playerControllers;
    public GameObject playerBasicPrefab;

    public int SceneToLoadNumber = -1;
    public int lastScene;

    public bool gameByTime = false;

    public Animation SceneTransition;

    private bool subscribed = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += StartGame;
    }

    public IEnumerator LoadNewGame()
    {
        if (SceneTransition) SceneTransition.Play(SceneTransition.clip.name);

        if (SceneTransition) yield return new WaitForSeconds(SceneTransition.clip.length);

        lastScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(SceneToLoadNumber);

        if (players[0]) foreach (PlayerSelectionPanel player in players) { player.GameStarting = true; }
    }

    private void CreatePlayer(PlayerSelectionPanel playerInfo)
    {
        PlayerController player = Instantiate(playerBasicPrefab).GetComponent<PlayerController>();

        player.inputControl.controllerNumber = playerInfo.controllerNumber;
        player.inputControl.keyboardAndMouse = playerInfo.controllerNumber == 5;
        player.playerNumber = playerInfo.playerNumber;
        player.SetSkin(playerInfo.playerSkin);
        player.enabled = false;

        _LevelManager.instance.players.Add(player);
    }

    private void CreatePlayer(PlayerController playerInfo)
    {
        PlayerController player = Instantiate(playerBasicPrefab).GetComponent<PlayerController>();

        player.inputControl.controllerNumber = playerInfo.inputControl.controllerNumber;
        player.inputControl.keyboardAndMouse = playerInfo.inputControl.keyboardAndMouse;
        player.playerNumber = playerInfo.playerNumber;
        player.SetSkin(playerInfo.playerSkin);
        player.enabled = false;

        _LevelManager.instance.players.Add(player);
    }

    public void StartGame(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != SceneToLoadNumber) return;

        if (!_LevelManager.instance.tutorialScene) SceneManager.sceneLoaded -= StartGame;

        _LevelManager.instance.testingScene = false;

        foreach (PlayerController player in _LevelManager.instance.players)
        {
            player.gameObject.SetActive(false);
        }

        _LevelManager.instance.players.Clear();

        _LevelManager.instance.testingScene = false;
        _LevelManager.instance.CheckTest();

        foreach (PlayerController player in _LevelManager.instance.players) { player.gameObject.SetActive(false); }

        _LevelManager.instance.players.Clear();

        if (playerControllers.Count == 0) foreach (PlayerSelectionPanel player in players) { CreatePlayer(player); }
        else foreach (PlayerController player in playerControllers) { CreatePlayer(player); }

        _LevelManager.instance.SetNewGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public IEnumerator LoadNextScene()
    {
        SceneTransition.Play(SceneTransition.clip.name);

        yield return new WaitForSeconds(SceneTransition.clip.length);

        if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetGameMode(bool killHoarder)
    {
        gameByTime = killHoarder;

        StartCoroutine(LoadNextScene());
    }
}

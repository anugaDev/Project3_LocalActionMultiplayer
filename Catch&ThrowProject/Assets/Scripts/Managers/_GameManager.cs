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

    public bool gameByTime = false;

    public Animation SceneTransition;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += StartGame;
    }

    public IEnumerator LoadNewGame()
    {
        SceneTransition.Play(SceneTransition.clip.name);

        yield return new WaitForSeconds(SceneTransition.clip.length);

        lastScene = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadSceneAsync(SceneToLoadNumber);

        foreach (PlayerSelectionPanel player in players) { player.GameStarting = true; }
    }

    private void CreatePlayer(PlayerSelectionPanel playerInfo)
    {
        GameObject player = Instantiate(playerBasicPrefab);

        player.GetComponent<InputController>().controllerNumber = playerInfo.controllerNumber;
        player.GetComponent<InputController>().keyboardAndMouse = playerInfo.controllerNumber == 5 ? true : false;
        player.GetComponent<PlayerController>().SetSkin(playerInfo.playerSkin);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerController>().playerNumber = playerInfo.playerNumber;

        _LevelManager.instance.players.Add(player.GetComponent<PlayerController>());
    }

    public void StartGame(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != SceneToLoadNumber) return;

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

        foreach (PlayerSelectionPanel player in players) { CreatePlayer(player); }

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

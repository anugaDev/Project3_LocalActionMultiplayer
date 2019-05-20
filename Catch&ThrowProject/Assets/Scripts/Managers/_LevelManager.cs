using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class _LevelManager : MonoBehaviour
{
    public static _LevelManager instance;

    public List<Transform> spawnPoints;
    public List<PlayerController> players;

    public StandaloneInputModule UI_Input;

    public DynamicCamera cameraFollow;

    [Header("Match Settings")]
    public int StartingLifes = 10;
    public int MatchDuration = 120;

    public bool matchByTime = false;
    private float gameTimer = 0f;

    [Header("UI Elements")]
    public GameObject UI_Parent;
    public GameObject PauseMenu;
    public GameObject EndMenu;
    public GameObject playerPanelPrefab;

    public bool testingScene;

    public enum MatchState
    {
        Starting,
        Playing,
        Ending,
        Paused
    }

    public MatchState matchState;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    private void Start()
    {
        if (testingScene)
        {
            var playerPrefs = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in playerPrefs)
            {
                players.Add(player.GetComponent<PlayerController>());
                if (!player.activeSelf) player.SetActive(true);
            }

            CheckTest();
            SetNewGame();
        }
    }

    void Update()
    {
        if (matchByTime && matchState == MatchState.Playing)
        {
            gameTimer += Time.deltaTime;

            if (gameTimer >= MatchDuration)
            {
                gameTimer = 0;

                EndMatch();
            }
        }
    }

    private void StartGame()
    {
        cameraFollow.enabled = true;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].health = StartingLifes;
            players[i].uiPanel = Instantiate(playerPanelPrefab, parent: UI_Parent.transform).GetComponent<UpdatePlayerPanel>();
            players[i].uiPanel.RemoveLife(players[i].health);
            players[i].enabled = true;
        }

        Time.timeScale = 1;

        matchState = MatchState.Playing;
    }

    private void SetInputUI(int playerNumber)
    {
        UI_Input.horizontalAxis = "Horizontal" + playerNumber;
        UI_Input.verticalAxis = "Vertical" + playerNumber;

        UI_Input.submitButton = "Jump" + playerNumber;
        UI_Input.cancelButton = "Fire" + playerNumber;
    }

    public void SetNewGame()
    {
        matchState = MatchState.Starting;

        for (int i = 0; i < players.Count; i++)
        {
            SpawnPlayer(players[i].gameObject, i);

            cameraFollow.objectsToShow.Add(players[i].transform);
        }

        //Here you would wait until counter goes down or some visual effect
        //StartCoroutine(StartCountdown()); -HALF IMPLEMENTED-

        StartGame();
    }

    public void SpawnPlayer(GameObject player, int? position)
    {
        if (spawnPoints.Count == 0) return;

        Vector3 spawnPosition;

        spawnPosition = position.HasValue ? spawnPoints[position.Value].position :
                                            spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count - 1)].position;

        player.transform.position = spawnPosition;
    }

    public void PauseGame(int playerNumber)
    {
        matchState = MatchState.Paused;

        SetInputUI(playerNumber);

        PauseMenu.SetActive(true);

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);

        Time.timeScale = 1;

        matchState = MatchState.Playing;
    }

    public void ResetGame()
    {

    }

    public void OnPlayerKilled(PlayerController player)
    {
        if (player.health == 0) return;

        player.health--;
        player.uiPanel.RemoveLife(player.health);

        if (player.health == 0 && !matchByTime)
        {
            cameraFollow.objectsToShow.Remove(player.transform);
            player.gameObject.SetActive(false);

            int alivePlayers = 0;

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].health > 0) alivePlayers++;
            }

            if (alivePlayers == 1) EndMatch();
        }
    }

    private void EndMatch()
    {
        matchState = MatchState.Ending;

        EndMenu.SetActive(true);
        //Implement the end of the game.
    }

    public void CheckTest()
    {
        players.Clear();

        var playerPrefs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playerPrefs)
        {
            players.Add(player.GetComponent<PlayerController>());
            if (!player.activeSelf) player.SetActive(true);
        }
    }

    public IEnumerator StartCountdown()
    {
        //Show 3

        yield return new WaitForSeconds(1);

        //Show 2

        yield return new WaitForSeconds(1);

        //Show 1

        yield return new WaitForSeconds(1);

        //Show Blitz

        yield return new WaitForSeconds(1);

        StartGame();
    }
}

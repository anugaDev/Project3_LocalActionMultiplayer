using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _LevelManager : MonoBehaviour
{
    public static _LevelManager instance;

    public List<Transform> spawnPoints;
    public List<PlayerController> players;

    public DynamicCamera cameraFollow;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    public void SetNewGame()
    {
        for (int i = 0; i < players.Count; i++)
        {
            SpawnPlayer(players[i].gameObject, i);

            cameraFollow.objectsToShow.Add(players[i].transform);
        }

        //Here you would wait until counter goes down or some visual effect

        StartGame();
    }

    private void StartGame()
    {
        cameraFollow.enabled = true;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].enabled = true;
        }
    }

    public void SpawnPlayer(GameObject player, int position)
    {
        player.transform.position = spawnPoints[position].position;
    }

    public void SpawnPlayer(GameObject player)
    {
        int index = UnityEngine.Random.Range(0, spawnPoints.Count - 1);

        player.transform.position = spawnPoints[index].position;
    }
}

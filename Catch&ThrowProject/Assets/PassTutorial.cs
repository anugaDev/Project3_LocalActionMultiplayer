using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTutorial : MonoBehaviour
{
    void Update()
    {
        foreach (PlayerController player in _LevelManager.instance.players)
        {
            if (Input.GetButtonDown("Start" + player.inputControl.controllerNumber))
            {
                _LevelManager.instance.matchState = _LevelManager.MatchState.Ending;

                _GameManager.instance.StartCoroutine(_GameManager.instance.LoadNewGame());

                Destroy(_LevelManager.instance.gameObject);
            }
        }
    }
}

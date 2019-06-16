using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTutorial : MonoBehaviour
{
    bool passed;

    void Update()
    {
        if (passed) return;

        foreach (PlayerController player in _LevelManager.instance.players)
        {
            if (Input.GetButtonDown("Start" + player.inputControl.controllerNumber))
            {
                passed = true;

                _LevelManager.instance.matchState = _LevelManager.MatchState.Ending;

                _GameManager.instance.StartCoroutine(_GameManager.instance.LoadNewGame());

                Destroy(_LevelManager.instance.gameObject);
            }
        }
    }
}

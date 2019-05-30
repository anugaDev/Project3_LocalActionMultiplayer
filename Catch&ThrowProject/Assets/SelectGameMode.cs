using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectGameMode : MonoBehaviour
{
    public string leftButton = "LB";
    public string rightButton = "RB";

    void Update()
    {
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetButtonDown(leftButton + i))
            {
                _GameManager.instance.gameByTime = false;
                _GameManager.instance.LoadNextScene();
            }

            if (Input.GetButtonDown(rightButton + i))
            {
                _GameManager.instance.gameByTime = true;
                _GameManager.instance.LoadNextScene();
            }
        }
    }
}

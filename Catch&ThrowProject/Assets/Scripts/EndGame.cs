using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndGame : MonoBehaviour
{
    public void GoEnd()
    {
        Time.timeScale = 1;
        _GameManager.instance.GoToMainMenu();
    }
}

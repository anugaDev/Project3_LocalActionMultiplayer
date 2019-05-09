using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndGame : MonoBehaviour
{
    public void GoEnd()
    {
        _GameManager.instance.GoToMainMenu();
    }
}

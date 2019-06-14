using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssignToGameManger : MonoBehaviour
{
    void Start()
    {
        _GameManager.instance.SceneTransition = GetComponent<Animation>();
    }
}

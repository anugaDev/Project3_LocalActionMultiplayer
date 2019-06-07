using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToGameManger : MonoBehaviour
{
    void Start()
    {
        _GameManager.instance.SceneTransition = GetComponent<Animation>();
    }
}

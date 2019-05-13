using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;

    public float timeToSpawn = 30f;

    private GameObject lastSpawnedObject = null;

    private float timer = 0f;

    void Update()
    {
        if (!lastSpawnedObject)
        {
            timer += Time.deltaTime;

            if (timer >= timeToSpawn)
            {
                lastSpawnedObject = Instantiate(objectToSpawn);

                timer = 0f;
            }
        }
    }
}

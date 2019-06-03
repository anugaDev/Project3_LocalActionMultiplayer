using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;

    public float timeToSpawn = 30f;

    private GameObject lastSpawnedObject = null;

    public List<GameObject> spawners;

    private float timer = 0f;

    void Update()
    {
        if (!lastSpawnedObject)
        {
            timer += Time.deltaTime;

            if (timer >= timeToSpawn)
            {
                lastSpawnedObject = Instantiate(objectToSpawn, spawners[Random.Range(0, spawners.Count)].transform.position, Quaternion.identity);

                timer = 0f;
            }
        }
    }
}

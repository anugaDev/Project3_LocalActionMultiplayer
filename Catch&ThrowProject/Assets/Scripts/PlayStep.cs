using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlayStep : MonoBehaviour
{
    [FMODUnity.EventRef] public string[] steps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Step()
    {
        RuntimeManager.PlayOneShot(steps[Random.Range(0,steps.Length)]);
    }
}

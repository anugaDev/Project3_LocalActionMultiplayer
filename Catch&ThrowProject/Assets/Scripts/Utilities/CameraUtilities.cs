﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtilities : MonoBehaviour
{
    [SerializeField] private FMODUnity.StudioListener listener;
    [SerializeField] private float trueCameraListenerRadius;
    public static CameraUtilities instance;

    [SerializeField] private float originalShakeForce;
    [SerializeField] private Camera sceneCamera;

    private Vector3 estimatedCameraCenter;
    private float actualShakeForce;
    private Vector3 originalCameraPosition;
    private Vector2 moveDirection;
    
    

    private void Awake()
    {
//        listener.
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        originalCameraPosition = sceneCamera.transform.position;
        estimatedCameraCenter = originalCameraPosition;
    }

    public void ShakeCamera(float shakeTime, float shakeForce)
    {
        StartCoroutine(CameraShake(shakeTime, shakeForce));
    }

    public void MoveCamera(Vector3 direction, float force, float time)
    {
        StartCoroutine(MoveTowardsDirection(direction, force, time));
    }

    private IEnumerator CameraShake(float time, float force)
    {
        var actualTime = 0f;

        while (actualTime < time)
        {
            var randomUnity = Random.insideUnitSphere;
            randomUnity.z = 0;
            sceneCamera.transform.localPosition = (randomUnity * force);
            
            yield return null;
            actualTime += Time.deltaTime;
        }

        sceneCamera.transform.localPosition = Vector3.zero;
    }
//    var randomUnity = Random.insideUnitSphere;
//    randomUnity.z = 0;
//    var newPos = estimatedCameraCenter + (randomUnity * force);
//    newPos.z = 0f;
//    sceneCamera.transform.position = newPos;
    private IEnumerator MoveTowardsDirection(Vector3 direction, float force, float time)
    {
        var actualTime = 0;
        var dir = direction;

        while (actualTime < time)
        {
            if (actualTime > time / 2) dir = -direction;

            estimatedCameraCenter += dir * force * Time.deltaTime;

            yield return null;
        }

        estimatedCameraCenter = originalCameraPosition;

        sceneCamera.transform.position = estimatedCameraCenter;
    }

    public void StopCameraMovement()
    {

    }
}

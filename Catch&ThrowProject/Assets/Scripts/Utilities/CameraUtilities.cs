using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtilities : MonoBehaviour
{
    public static CameraUtilities instance;
    
    [SerializeField] private float originalShakeForce;
    [SerializeField] private Camera sceneCamera;
    
    private Vector3 estimatedCameraCenter;
    private float actualShakeForce;
    private Vector3 originalCameraPosition;
    private Vector2 moveDirection;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    void Start()
    {
        originalCameraPosition = sceneCamera.transform.position;
        estimatedCameraCenter = originalCameraPosition;
    }
    void Update()
    {
        
    }

    public void ShakeCamera(float shakeTime, float shakeForce)
    {
        StartCoroutine(CameraShake(shakeTime, shakeForce));
    }

    private IEnumerator CameraShake(float time, float force)
    {
        var actualTime = 0f;

        while (actualTime < time)
        {
            sceneCamera.transform.position = estimatedCameraCenter + (Vector3)(Random.insideUnitCircle * force);
            yield return null;
            actualTime++;
        }

        sceneCamera.transform.position = estimatedCameraCenter;
    }

    private IEnumerator MoveTowardsDirection(Vector2 direction, float force)
    {
       
        var actualTime = 0f;
        var pathMade = false;

        while (!pathMade)
        {
            yield return null;
        }

        estimatedCameraCenter = originalCameraPosition;
        
        sceneCamera.transform.position = estimatedCameraCenter;

    }

    public void StopCameraMovement()
    {
        
    }
    
}

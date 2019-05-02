using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public List<Transform> objectsToShow;

    private Vector3 desiredPosition;
    private Vector3 desiredZoom;

    public float positionDamping;

    public float minZoom = 20f;
    public float maxZoom = 45f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;

    private Bounds bounds;
    private Camera myCamera;

    private void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Move();
        Zoom();
    }

    private void Move()
    {
        desiredPosition = CenterOfMass(objectsToShow);
        desiredPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionDamping);
    }

    private void Zoom()
    {
        float desiredZoom = Mathf.Lerp(minZoom, maxZoom, bounds.size.x / zoomLimiter);

        myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, desiredZoom, Time.deltaTime);
    }

    private Vector3 CenterOfMass(List<Transform> objects)
    {
        bounds = new Bounds(objects[0].position, Vector3.zero);

        foreach (Transform obj in objects)
        {
            bounds.Encapsulate(obj.position);
        }

        return bounds.center;
    }
}

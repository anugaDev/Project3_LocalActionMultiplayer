using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    //CONSTRUCTION PROCESS. NOT WORKING RIGHT NOW, DON'T TEST
    public List<Transform> objectsToShow;

    private Vector3 desiredPosition;
    private Vector3 desiredZoom;

    public float positionDamping;
    public float zoomDamping;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Vector3 velocity;

    void Start()
    {

    }

    void LateUpdate()
    {
        GetDesiredPosition();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionDamping);
    }

    private void GetDesiredPosition()
    {
        desiredPosition = CenterOfMass(objectsToShow);

        desiredPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);
        desiredPosition.z = transform.position.z;
    }

    private void GetDesiredZoom()
    {

    }

    public Vector3 CenterOfMass(List<Transform> positions)
    {
        Vector3 centerOfMass = new Vector3();

        for (int i = 0; i < positions.Count; i++)
        {
            centerOfMass += positions[i].position;
        }

        return centerOfMass / (positions.Count);
    }
}

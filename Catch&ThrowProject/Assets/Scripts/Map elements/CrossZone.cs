using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossZone : MonoBehaviour
{
    [SerializeField] private CrossZone exitingZone;
    [SerializeField] private float verifiedCrossedDistance;
    private Transform lastObjectCrossed;
    private bool crossed;

    public void ObjectCross(Transform objectTransform)
    {
        if (crossed) return;

        var localPos = transform.InverseTransformPoint(objectTransform.position);
        exitingZone.crossed = true;
        objectTransform.position = exitingZone.transform.TransformPoint(localPos);
        exitingZone.lastObjectCrossed = objectTransform;
    }

    private void Update()
    {
        if (!lastObjectCrossed) return;

        if ((lastObjectCrossed.transform.position - transform.position).magnitude > verifiedCrossedDistance)
        {
            crossed = false;
            lastObjectCrossed = null;
        }
    }
}

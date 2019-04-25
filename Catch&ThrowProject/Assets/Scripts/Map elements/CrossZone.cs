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
        if (!crossed)
        {
            var localPos = transform.InverseTransformPoint(objectTransform.position);
            exitingZone.crossed = true;
            objectTransform.position = exitingZone.transform.TransformPoint(localPos);
            exitingZone.lastObjectCrossed = objectTransform;
        }
    }

    private void Update()
    {
        if (lastObjectCrossed != null)
        {
            if ((lastObjectCrossed.transform.position - transform.position).magnitude > verifiedCrossedDistance)
            {
                print("CROSSED");
                crossed = false;
                lastObjectCrossed = null;

            }
        }
    }
}

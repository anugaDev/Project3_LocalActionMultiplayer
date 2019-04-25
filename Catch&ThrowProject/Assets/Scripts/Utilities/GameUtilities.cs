using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtilities
{
    public IEnumerator ShakeObject(float time, Transform objectToShake, float force)
    {
        var originalPos = objectToShake.position;
        var actualTime = 0f;
        
        while (actualTime < time)
        {
            var randomUnity = Random.insideUnitSphere;
            randomUnity.z = 0;
            objectToShake.position = originalPos + (randomUnity * force);
            yield return null;
            actualTime+= Time.deltaTime;
        }
        objectToShake.position = originalPos;
    }

    public IEnumerator ScaleTime(float time)
    {
        yield return null;
    }
}

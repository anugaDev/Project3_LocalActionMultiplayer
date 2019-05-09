using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTimeControl : MonoBehaviour
{
    [SerializeField] private float trainPassMinTime;
    [SerializeField] private float trainPassMaxTime;
    [SerializeField] private float beforeTrainAffordance;
    [SerializeField] private float passShakeForce;

    [SerializeField] private Animation train;
    [SerializeField] private Animation affordance;
    [SerializeField] private bool playOnStart;

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart) StartCoroutine(CountTimeForTrain(0, 0));
        else PreparePass();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PreparePass()
    {
        var passTime = Random.Range(trainPassMinTime, trainPassMaxTime);
        StartCoroutine(CountTimeForTrain(passTime, passTime - beforeTrainAffordance));
    }

    private IEnumerator CountTimeForTrain(float passTime, float affordanceTime)
    {
        var actualPassTime = 0f;

        while (actualPassTime < passTime)
        {
            yield return new WaitForEndOfFrame();
            actualPassTime += Time.deltaTime;
            if (actualPassTime >= affordanceTime && !affordance.isPlaying)
            {
                affordance.gameObject.SetActive(true);
                affordance.Play();
            }

        }

        train.Play();
        CameraUtilities.instance.ShakeCamera(train.clip.length, passShakeForce);

        while (train.isPlaying)
        {
            yield return null;
        }
        affordance.Stop();
        affordance.gameObject.SetActive(false);
        train.Stop();
        PreparePass();
    }
}

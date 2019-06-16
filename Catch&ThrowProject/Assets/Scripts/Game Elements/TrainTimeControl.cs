using System.Collections;
using System.Collections.Generic;
using FMODUnity;
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
    
    
    [FMODUnity.EventRef] public string bellSound;
    private FMOD.Studio.EventInstance bellsoundEvent;
    
    [FMODUnity.EventRef] public string trainSound;
    private FMOD.Studio.EventInstance trainSoundevent;
    // Start is called before the first frame update
    void Start()
    {
        bellsoundEvent = RuntimeManager.CreateInstance(bellSound);
        trainSoundevent = RuntimeManager.CreateInstance(trainSound);
        
        if (playOnStart) StartCoroutine(CountTimeForTrain(0, 0));
        else PreparePass();
    }

    // Update is called once per frame
    void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(bellsoundEvent,affordance.transform, affordance.GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(trainSoundevent,train.transform, train.GetComponent<Rigidbody>());

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
                bellsoundEvent.start();
            }

        }

        train.Play();
        trainSoundevent.start();
        CameraUtilities.instance.ShakeCamera(train.clip.length, passShakeForce);

        while (train.isPlaying)
        {
            yield return null;
        }
        affordance.Stop();
        affordance.gameObject.SetActive(false);
        bellsoundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        trainSoundevent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);      
        PreparePass();
    }
}

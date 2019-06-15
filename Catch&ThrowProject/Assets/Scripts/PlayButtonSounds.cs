using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonSounds : MonoBehaviour
{
    [SerializeField] private Button menuButton;

    [FMODUnity.EventRef] public string clickButton;
        
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        menuButton.onClick.AddListener(() => RuntimeManager.PlayOneShot(clickButton));
    }
}

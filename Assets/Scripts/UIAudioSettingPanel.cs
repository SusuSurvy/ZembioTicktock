using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioSettingPanel : MonoBehaviour
{
    public Slider BgVolumeSlider;
    public Slider AudioVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {

        float bgVolume = PlayerPrefs.GetFloat("BgVolume", 1);
        float audioVolume = PlayerPrefs.GetFloat("AudioVolume", 1);
        GameDataInstance.Instance.BgAudioVolume = bgVolume;
        GameDataInstance.Instance.AudioVolume = audioVolume;
        BgVolumeSlider.value = bgVolume;
        AudioVolumeSlider.value = audioVolume;
        BgVolumeSlider.onValueChanged.AddListener((value) =>
        {
            GameDataInstance.Instance.BgAudioVolume = value;
            PlayerPrefs.SetFloat("BgVolume", value);
        });
        AudioVolumeSlider.onValueChanged.AddListener((value) =>
        {
            GameDataInstance.Instance.AudioVolume = value;
            PlayerPrefs.SetFloat("AudioVolume", value);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

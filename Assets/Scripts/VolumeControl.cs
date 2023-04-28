using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] float volumeStart;
    const string VOLUME_KEY = "VolumeNumber";
    const float V_START = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(VOLUME_KEY)) {
            volumeStart = PlayerPrefs.GetFloat(VOLUME_KEY);
            AudioListener.volume = volumeStart;
            volumeSlider.value = volumeStart;
        }
        else
        {
            PlayerPrefs.SetFloat(VOLUME_KEY, V_START);
        }

    }

    // Update is called once per frame
    void Update()
    {
        OnValueChanged();
    }

    public void OnValueChanged()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat(VOLUME_KEY, volumeSlider.value);
    }

}

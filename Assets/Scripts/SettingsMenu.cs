using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle fullscreenToggle;

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.8f);
        fullscreenToggle.isOn = Screen.fullScreen;
        SetVolume(volumeSlider.value);
    }

    public void SetVolume(float v)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(v) * 20); // slider -> decibels
        PlayerPrefs.SetFloat("Volume", v);
    }

    public void SetFullscreen(bool on) => Screen.fullScreen = on;
}
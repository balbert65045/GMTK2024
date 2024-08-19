using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject Canvas;

    [SerializeField] AudioMixer MasterMixer;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    // Start is called before the first frame update
    void Start()
    {
        float musicStartVal;
        MasterMixer.GetFloat("musicVol", out musicStartVal);
        musicSlider.value = convertDecibalsToFloating(musicStartVal);

        float effectsStartVal;
        MasterMixer.GetFloat("effectsVol", out effectsStartVal);
        SFXSlider.value = musicSlider.value = convertDecibalsToFloating(effectsStartVal);

        musicSlider.onValueChanged.AddListener(delegate { MusicVolChanged(); });
        SFXSlider.onValueChanged.AddListener(delegate { EffectsVolChanged(); });
    }

    void MusicVolChanged()
    {
        MasterMixer.SetFloat("musicVol", convertFloatingPointToDecibals(musicSlider.value));
    }

    void EffectsVolChanged()
    {
        MasterMixer.SetFloat("effectsVol", convertFloatingPointToDecibals(SFXSlider.value));
    }

    float convertFloatingPointToDecibals(float value)
    {
        return (30 * value) - 30;
    }

    float convertDecibalsToFloating(float decibals)
    {
        return (decibals + 30) / 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.Instance.Paused)
            {
                PuaseGameAndOpenMenu();
            }
            else
            {
                ResumeGameAndCloseMenu();
            }
        }
    }

    void PuaseGameAndOpenMenu()
    {
        GameManager.Instance.SetPaused(true);
        Time.timeScale = 0f;
        Canvas.SetActive(true);
    }

    public void ResumeGameAndCloseMenu()
    {
        GameManager.Instance.SetPaused(false);
        Time.timeScale = 1f;
        Canvas.SetActive(false);
    }
}

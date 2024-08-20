using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenMusic : MonoBehaviour
{
    [SerializeField] AudioSource titleScreenMusic;

    private void Awake()
    {
        
        AudioManager.Instance.Level1Audio.Stop();
        AudioManager.Instance.Level2Audio.Stop();
        AudioManager.Instance.Level3Audio.Stop();
        AudioManager.Instance.Level4Audio.Stop();
        AudioManager.Instance.GodModeAudio.Stop();
    }


}

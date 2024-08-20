using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudioController : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Level1GodSpot;
    [SerializeField] GameObject Level2GodSpot;
    [SerializeField] GameObject Level3GodSpot;
    [SerializeField] GameObject Level4GodSpot;


    private void Awake()
    {
        AudioManager.Instance.Level1Audio.volume = 1;
        if (Level2GodSpot == null)
        {
            AudioManager.Instance.Level1Audio.PlayScheduled(0);
            return;
        }
        AudioManager.Instance.Level1Audio.PlayScheduled(0);
        AudioManager.Instance.Level2Audio.PlayScheduled(0);
        AudioManager.Instance.Level3Audio.PlayScheduled(0);
        AudioManager.Instance.Level4Audio.PlayScheduled(0);
        AudioManager.Instance.GodModeAudio.PlayScheduled(0);
    }

    private void Update()
    {
        if (Level2GodSpot == null)
        {
            if (!AudioManager.Instance.Level1Audio.isPlaying) { AudioManager.Instance.Level1Audio.Play(); }
            return;
        }
        if (!AudioManager.Instance.Level1Audio.isPlaying && !AudioManager.Instance.Level2Audio.isPlaying && !AudioManager.Instance.Level3Audio.isPlaying && !AudioManager.Instance.Level4Audio.isPlaying)
        {
            AudioManager.Instance.GodModeAudio.Play();
            AudioManager.Instance.Level1Audio.Play();
            AudioManager.Instance.Level2Audio.Play();
            AudioManager.Instance.Level3Audio.Play();
            AudioManager.Instance.Level4Audio.Play();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Level2GodSpot == null) { return; }
        if (GameManager.Instance.Mode == GameMode.PLATFORM_MODE)
        {
            AudioManager.Instance.GodModeAudio.volume = Mathf.Lerp(AudioManager.Instance.GodModeAudio.volume, 0, 20f * Time.fixedDeltaTime);
            if (Player.transform.position.y < Level2GodSpot.transform.position.y)
            {
                float percentageToLevel2 = Mathf.Clamp(Player.transform.position.y - Level1GodSpot.transform.position.y, 0, float.MaxValue) / (Level2GodSpot.transform.position.y - Level1GodSpot.transform.position.y);
                AudioManager.Instance.Level1Audio.volume = 1 - percentageToLevel2;
                AudioManager.Instance.Level2Audio.volume = percentageToLevel2;
                AudioManager.Instance.Level3Audio.volume = 0;
                AudioManager.Instance.Level4Audio.volume = 0;
            }
            else if (Player.transform.position.y < Level3GodSpot.transform.position.y)
            {
                float percentageToLevel3 = Mathf.Clamp(Player.transform.position.y - Level2GodSpot.transform.position.y, 0, float.MaxValue) / (Level3GodSpot.transform.position.y - Level2GodSpot.transform.position.y);
                AudioManager.Instance.Level1Audio.volume = 0;
                AudioManager.Instance.Level4Audio.volume = 0;
                AudioManager.Instance.Level2Audio.volume = 1 - percentageToLevel3;
                AudioManager.Instance.Level3Audio.volume = percentageToLevel3;
            }
            else if (Player.transform.position.y < Level4GodSpot.transform.position.y)
            {
                float percentageToLevel4 = Mathf.Clamp(Player.transform.position.y - Level3GodSpot.transform.position.y, 0, float.MaxValue) / (Level4GodSpot.transform.position.y - Level3GodSpot.transform.position.y);
                AudioManager.Instance.Level1Audio.volume = 0;
                AudioManager.Instance.Level2Audio.volume = 0;
                AudioManager.Instance.Level3Audio.volume = 1 - percentageToLevel4;
                AudioManager.Instance.Level4Audio.volume = percentageToLevel4;
            }
            else
            {
                AudioManager.Instance.Level1Audio.volume = 0;
                AudioManager.Instance.Level2Audio.volume = 0;
                AudioManager.Instance.Level3Audio.volume = 0;
                AudioManager.Instance.Level4Audio.volume = 1;
            }
        }
        else
        {
            AudioManager.Instance.Level1Audio.volume = Mathf.Lerp(AudioManager.Instance.Level1Audio.volume, 0, 20f * Time.fixedDeltaTime);
            AudioManager.Instance.Level2Audio.volume = Mathf.Lerp(AudioManager.Instance.Level2Audio.volume, 0, 20f * Time.fixedDeltaTime);
            AudioManager.Instance.Level3Audio.volume = Mathf.Lerp(AudioManager.Instance.Level3Audio.volume, 0, 20f * Time.fixedDeltaTime);
            AudioManager.Instance.Level4Audio.volume = Mathf.Lerp(AudioManager.Instance.Level4Audio.volume, 0, 20f * Time.fixedDeltaTime);
            AudioManager.Instance.GodModeAudio.volume = Mathf.Lerp(AudioManager.Instance.GodModeAudio.volume, 1, 20f * Time.fixedDeltaTime);
        }
    }
}

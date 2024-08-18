using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource GodModeAudio;
    [SerializeField] AudioSource Level1Audio;
    [SerializeField] AudioSource Level2Audio;
    [SerializeField] AudioSource Level3Audio;
    [SerializeField] AudioSource Level4Audio;



    [SerializeField] GameObject Player;
    [SerializeField] GameObject Level1GodSpot;
    [SerializeField] GameObject Level2GodSpot;
    [SerializeField] GameObject Level3GodSpot;
    [SerializeField] GameObject Level4GodSpot;


    // Start is called before the first frame update
    void Start()
    {
    }
    private void Update()
    {
        if (!Level1Audio.isPlaying && !Level2Audio.isPlaying && !Level3Audio.isPlaying && !Level4Audio.isPlaying)
        {
            GodModeAudio.Play();
            Level1Audio.Play();
            Level2Audio.Play();
            Level3Audio.Play();
            Level4Audio.Play();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.Mode == GameMode.PLATFORM_MODE)
        {
            GodModeAudio.volume = Mathf.Lerp(GodModeAudio.volume, 0, 20f*Time.fixedDeltaTime);
            if (Player.transform.position.y < Level2GodSpot.transform.position.y)
            {
                float percentageToLevel2 = Mathf.Clamp(Player.transform.position.y - Level1GodSpot.transform.position.y, 0, float.MaxValue) / (Level2GodSpot.transform.position.y - Level1GodSpot.transform.position.y);
                Level1Audio.volume = 1 - percentageToLevel2;
                Level2Audio.volume = percentageToLevel2;
                Level3Audio.volume = 0;
                Level4Audio.volume = 0;
            }
            else if (Player.transform.position.y < Level3GodSpot.transform.position.y)
            {
                float percentageToLevel3 = Mathf.Clamp(Player.transform.position.y - Level2GodSpot.transform.position.y, 0, float.MaxValue) / (Level3GodSpot.transform.position.y - Level2GodSpot.transform.position.y);
                Level1Audio.volume = 0;
                Level4Audio.volume = 0;
                Level2Audio.volume = 1 - percentageToLevel3;
                Level3Audio.volume = percentageToLevel3;
            }
            else if (Player.transform.position.y < Level4GodSpot.transform.position.y)
            {
                float percentageToLevel4 = Mathf.Clamp(Player.transform.position.y - Level3GodSpot.transform.position.y, 0, float.MaxValue) / (Level4GodSpot.transform.position.y - Level3GodSpot.transform.position.y);
                Level1Audio.volume = 0;
                Level2Audio.volume = 0;
                Level3Audio.volume = 1 - percentageToLevel4;
                Level4Audio.volume = percentageToLevel4;
            }
            else
            {
                Level1Audio.volume = 0;
                Level2Audio.volume = 0;
                Level3Audio.volume = 0;
                Level4Audio.volume = 1;
            }
        }
        else
        {
            Level1Audio.volume = Mathf.Lerp(Level1Audio.volume, 0, 20f * Time.fixedDeltaTime);
            Level2Audio.volume = Mathf.Lerp(Level2Audio.volume, 0, 20f * Time.fixedDeltaTime);
            Level3Audio.volume = Mathf.Lerp(Level3Audio.volume, 0, 20f * Time.fixedDeltaTime);
            Level4Audio.volume = Mathf.Lerp(Level4Audio.volume, 0, 20f * Time.fixedDeltaTime);
            GodModeAudio.volume = Mathf.Lerp(GodModeAudio.volume, 1, 20f * Time.fixedDeltaTime);
        }
    }
}

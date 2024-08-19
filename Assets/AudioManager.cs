using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource flyBump;

    [SerializeField] AudioSource squeek;
    [SerializeField] AudioSource hiss;
    [SerializeField] AudioSource flyCatch;
    [SerializeField] AudioSource eatFly;
    [SerializeField] AudioSource freeFall;
    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource land;
    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource webLatch;
    [SerializeField] AudioSource webShot;
    [SerializeField] AudioSource webSwingLeft;
    [SerializeField] AudioSource webSwingRight;






    [SerializeField] AudioSource hotBarClick;
    [SerializeField] AudioSource moveAlongGrid;
    [SerializeField] AudioSource select;
    [SerializeField] AudioSource tilePlace;
    [SerializeField] AudioSource deleteTile;






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

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayFlyBump()
    {
        flyBump.Play();
    }

    public void DeleteBlock()
    {
        deleteTile.Play();
    }

    public void SwingWebLeft()
    {
        webSwingLeft.Play();
    }

    public void SwingWebRight()
    {
        webSwingRight.Play();
    }

    public void PlayWebShot()
    {
        webShot.Play();
    }

    public void PlayWebLatch()
    {
        webLatch.Play();
    }

    public void PlayWalk()
    {
        if (!walk.isPlaying)
        {
            walk.Play();
        }
    }

    public void StopWalk()
    {
        if (walk.isPlaying)
        {
            walk.Stop();
        } 
    }

    public void PlayTilePlace()
    {
        tilePlace.Play();
    }

    public void PlaySqueek()
    {
        squeek.Play();
    }

    public void PlayHiss()
    {
        hiss.Play();
    }

    public void PlaySelect()
    {
        select.Play();
    }

    public void MoveAlongGrid()
    {
        moveAlongGrid.Play();
    }

    public void PlayLand()
    {
        land.Play();
    }

    public void PlayJump()
    {
        jump.Play();
    }

    public void PlayHotBarClick()
    {
        hotBarClick.Play();
    }

    bool decreasingFreeFallVolume = false;
    bool increasingFreeFallVolume = false;
    float initialFreeFallVolume = 1;
    private void Start()
    {
        initialFreeFallVolume = freeFall.volume;
    }

    public void PlayFreeFall()
    {
        decreasingFreeFallVolume = false;
        increasingFreeFallVolume = true;
        freeFall.volume = 0;
        freeFall.Play();
        StartCoroutine("IncreasingVolume");
    }

    public void StopFreeFall()
    {
        increasingFreeFallVolume = false;
        if (freeFall.isPlaying)
        {
            decreasingFreeFallVolume = true;
            StartCoroutine("ReduceVolume");
        }
    }

    IEnumerator IncreasingVolume()
    {
        while (freeFall.isPlaying && increasingFreeFallVolume)
        {
            yield return new WaitForEndOfFrame();
            freeFall.volume = Mathf.Lerp(freeFall.volume, initialFreeFallVolume, 20 * Time.deltaTime);
        }
    }

    IEnumerator ReduceVolume()
    {
        while (freeFall.isPlaying && decreasingFreeFallVolume)
        {
            yield return new WaitForEndOfFrame();
            freeFall.volume = Mathf.Lerp(freeFall.volume, 0, 20 * Time.deltaTime);
        }
    }

    public void PlayFlyCatch()
    {
        flyCatch.Play();
    }


    public void PlayEatFly(float delay)
    {
        StartCoroutine("EatFly", delay);
    }


    IEnumerator EatFly(float delay)
    {
        yield return new WaitForSeconds(delay);
        eatFly.Play();
    }


    private void Update()
    {
        if (Level2GodSpot == null)
        {
            if (!Level1Audio.isPlaying) { Level1Audio.Play(); }
            return;
        }
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
        if (Level2GodSpot == null) { return; }
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

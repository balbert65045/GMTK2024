using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource winSound;


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





    [SerializeField] AudioSource buttonClick;
    [SerializeField] AudioSource hotBarClick;
    [SerializeField] AudioSource moveAlongGrid;
    [SerializeField] AudioSource select;
    [SerializeField] AudioSource tilePlace;
    [SerializeField] AudioSource deleteTile;






    public AudioSource GodModeAudio;
    public AudioSource Level1Audio;
    public AudioSource Level2Audio;
    public AudioSource Level3Audio;
    public AudioSource Level4Audio;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayButtonClick()
    {
        buttonClick.Play();
    }

    public void PlayWin()
    {
        winSound.Play();
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
}

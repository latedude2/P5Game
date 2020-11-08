using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] private AudioSource narrationPlayer;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource effectPlayer;

    private Subtitles subtitles;

    private AudioTrigger currentTrigger;

    private bool stopEffect;

    private void Start()
    {
        subtitles = GetComponentInChildren<Subtitles>();
        musicPlayer.loop = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AudioTrigger>() != null)
        {
            currentTrigger = other.GetComponent<AudioTrigger>();
            if (currentTrigger.isMusicTrigger)
            {
                //Debug.Log("Triggered music trigger");
                musicPlayer.clip = currentTrigger.triggerClip;
                musicPlayer.Play();
            }
            else
            {
                //Debug.Log("Triggered audio trigger");
                PrepareNarrative();
                narrationPlayer.Play();
                subtitles.Play();
                if (currentTrigger.changeMusic)
                {
                    musicPlayer.clip = currentTrigger.musicClip;
                    musicPlayer.Play();
                }
                stopEffect = currentTrigger.stopEffect;
            }
            if (currentTrigger.destroyOnPlay)
                currentTrigger.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Update()
    {
        if (stopEffect)
            StopEffect();
    }

    private void PrepareNarrative()
    {
        narrationPlayer.clip = currentTrigger.triggerClip;
        subtitles.SetupUpSubtitles(currentTrigger.subtitleText, currentTrigger.subtitleTime);
    }

    public void PlayAudioClip(AudioClip audioClip, bool isMusicClip, float volume = 1f)
    {
        if (isMusicClip)
        {
            musicPlayer.clip = audioClip;
            musicPlayer.volume = volume;
            musicPlayer.Play();
        } else
        {
            narrationPlayer.clip = audioClip;
            narrationPlayer.volume = volume;
            narrationPlayer.Play();
        }
    }

    public void PlayEffect(AudioClip audioClip, bool loop, float volume = 1f)
    {
        effectPlayer.clip = audioClip;
        effectPlayer.loop = loop;
        effectPlayer.volume = volume;
        effectPlayer.Play();
    }
    
    private void StopEffect()
    {
        if (effectPlayer.volume > 0f)
        {
            effectPlayer.volume -= 0.1f * Time.deltaTime;
        } else
        {
            effectPlayer.Stop();
            stopEffect = false;
        }
    }
}

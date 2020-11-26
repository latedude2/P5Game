using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float musicVolumeOnNarration;
    private float lastMusicVolume;
    [SerializeField, Range(0f, 1f)] private float effectVolumeOnNarration;
    private float lastEffectVolume;

    [SerializeField] private AudioSource narrationPlayer;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource effectPlayer;

    private Subtitles subtitles;

    private AudioTrigger currentTrigger;

    private void Start()
    {
        subtitles = GetComponentInChildren<Subtitles>();
        musicPlayer.loop = true;
        lastMusicVolume = musicPlayer.volume;
        lastEffectVolume = effectPlayer.volume;
    }

    private void Update()
    {
        if (!narrationPlayer.isPlaying)
        {
            StartCoroutine(FadeInEffect(musicPlayer, lastMusicVolume));
            StartCoroutine(FadeInEffect(effectPlayer, lastEffectVolume));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AudioTrigger>() != null)
        {
            currentTrigger = other.GetComponent<AudioTrigger>();
            
            PrepareNarrative();
            narrationPlayer.Play();
            subtitles.Play();

            LowerVolumeOnNarration();

            if (currentTrigger.changeMusic)
            {
                musicPlayer.clip = currentTrigger.musicClip;
                musicPlayer.Play();
            }

            if (currentTrigger.stopEffect)
            {
                //fade out the effect in a coroutine not to block all the other code
                StartCoroutine(FadeOutEffect());
            }
            //should the trigger be disabled after the first interaction
            if (currentTrigger.destroyOnPlay)
                currentTrigger.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void PrepareNarrative()
    {
        narrationPlayer.clip = currentTrigger.triggerClip;
        subtitles.SetupUpSubtitles(currentTrigger.subtitleText, currentTrigger.subtitleTime, currentTrigger.subtitleBackdropSize);
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
            LowerVolumeOnNarration();
        }
    }

    private void LowerVolumeOnNarration()
    {
        lastMusicVolume = musicPlayer.volume;
        lastEffectVolume = effectPlayer.volume;
        musicPlayer.volume *= musicVolumeOnNarration;
        effectPlayer.volume *= effectVolumeOnNarration;
    }

    public void PlayEffect(AudioClip audioClip, bool loop = true, float volume = 1f)
    {
        effectPlayer.clip = audioClip;
        effectPlayer.loop = loop;
        effectPlayer.volume = volume;
        effectPlayer.Play();
    }

    //used to get the fade out effect
    private IEnumerator FadeOutEffect()
    {
        while (effectPlayer.volume > 0f)
        {
            effectPlayer.volume -= 0.1f * Time.deltaTime;
            yield return null;
        }
        effectPlayer.Stop();
    }
    //used to get the fade out effect
    private IEnumerator FadeInEffect(AudioSource audioSource, float volumeLimit)
    {
        while (audioSource.volume < volumeLimit)
        {
            audioSource.volume += 0.1f * Time.deltaTime;
            yield return null;
        }
    }
}

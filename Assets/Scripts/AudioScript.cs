﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] private AudioSource narrationPlayer;
    [SerializeField] private AudioSource musicPlayer;

    private Subtitles subtitles;

    private AudioTrigger currentTrigger;

    private void Start()
    {
        subtitles = GetComponentInChildren<Subtitles>();
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
            }
            if (currentTrigger.destroyOnPlay)
                currentTrigger.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void PrepareNarrative()
    {
        narrationPlayer.clip = currentTrigger.triggerClip;
        subtitles.SetupUpSubtitles(currentTrigger.subtitleText, currentTrigger.subtitleTime);
    }

    public void PlayAudioClip(AudioClip audioClip, bool isMusicClip)
    {
        if (isMusicClip)
        {
            musicPlayer.clip = audioClip;
            musicPlayer.Play();
        } else
        {
            narrationPlayer.clip = audioClip;
            narrationPlayer.Play();
        }
    }
}

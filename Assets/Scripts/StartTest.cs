using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTest : MonoBehaviour
{
    [SerializeField] private AudioClip startMusicClip;

    [SerializeField] private bool isArrowCondition = false;

    [SerializeField] private NavigationalAid beeNavAid;
    [SerializeField] private NavigationalAid arrowNavAid;

    [SerializeField] private AudioClip introClipArrow;
    [SerializeField] private string[] subtitleTextArrow;
    [SerializeField] private float[] subtitleTimeArrow;

    [SerializeField] private AudioClip introClipBee;
    [SerializeField] private string[] subtitleTextBee;
    [SerializeField] private float[] subtitleTimeBee;

    private Subtitles subtitles;
    private AudioScript audioPlayer;

    void Start()
    {
        subtitles = GetComponentInChildren<Subtitles>();
        audioPlayer = GetComponent<AudioScript>();

        audioPlayer.PlayAudioClip(startMusicClip, true, 0.4f);

        if (isArrowCondition)
        {
            beeNavAid.DisableAidWithSound();
            subtitles.SetupUpSubtitles(subtitleTextArrow, subtitleTimeArrow);
            audioPlayer.PlayAudioClip(introClipArrow, false);
        } 
        else
        {
            arrowNavAid.DisableAidWithSound();
            subtitles.SetupUpSubtitles(subtitleTextBee, subtitleTimeBee);
            audioPlayer.PlayAudioClip(introClipBee, false);
        }
        subtitles.Play();
    }
}

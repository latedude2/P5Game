using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AudioScript : MonoBehaviour
{
    [SerializeField] private AudioSource narrationPlayer;
    [SerializeField] private AudioSource musicPlayer;

    [SerializeField] private TextMeshProUGUI textMesh;

    private AudioTrigger currentTrigger;
    private bool isNarrativePlaying = false;
    private float startTime;
    private int subtitleCounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        currentTrigger = other.GetComponent<AudioTrigger>();
        if (currentTrigger != null)
        {
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
            }
            if (currentTrigger.destroyOnPlay)
                Destroy(other);
        }
    }

    void FixedUpdate()
    {
        if (currentTrigger != null && currentTrigger.playSubtitles && isNarrativePlaying)
        {
            if (IsSubtitleOver(currentTrigger.subtitleTime[subtitleCounter]))
            {
                subtitleCounter++;
                SetTime();
            }
            
            if (subtitleCounter >= currentTrigger.subtitleText.Length)
            {
                isNarrativePlaying = false;
                textMesh.text = " ";
            } else
            {
                textMesh.text = currentTrigger.subtitleText[subtitleCounter];
            }
            
        }
    }

    private void PrepareNarrative()
    {
        narrationPlayer.clip = currentTrigger.triggerClip;
        SetTime();
        isNarrativePlaying = true;
    }

    private void SetTime()
    {
        startTime = Time.time;
    }

    private bool IsSubtitleOver(float subtitleTime)
    {
        return startTime + subtitleTime <= Time.time;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] private AudioSource narrationPlayer;
    [SerializeField] private AudioSource musicPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<AudioTrigger>() != null)
        { 
            if(other.GetComponent<AudioTrigger>().isMusicTrigger)
            {
                //Debug.Log("Triggered music trigger");
                musicPlayer.clip = other.GetComponent<AudioTrigger>().triggerClip;
                musicPlayer.Play();
            }
            else
            {
                //Debug.Log("Triggered audio trigger");
                narrationPlayer.clip = other.GetComponent<AudioTrigger>().triggerClip;
                narrationPlayer.Play();
            }
            if(other.GetComponent<AudioTrigger>().destroyOnPlay)
                Destroy(other);
        }
    }
}

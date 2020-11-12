using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndTaskInteraction : MonoBehaviour
{
    
    [SerializeField] private float honeyCollectionTime = 10;
    private float honeyCollectionTimeLeft;

    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject textElement;
    [SerializeField] private PlayerMovementRecorder playerMovementRecorder;

    [SerializeField] private float interactionDistance;

    private Slider slider;
    private bool taskCompleted = false;

    [SerializeField] private GameObject endTestTrigger;
    [SerializeField] private GameObject mistakeTriggerParent;
    [SerializeField] private GameObject shortcutTriggerParent;

    [SerializeField] private GameObject audioTriggers;

    [SerializeField] private AudioClip returnMusicClip;
    [SerializeField] private AudioClip returnEffect;

    [SerializeField] private AudioClip endTaskClip;
    [SerializeField] private string[] subtitleText;
    [SerializeField] private float[] subtitleTime;
    private Subtitles subtitles;
    private AudioScript audioPlayer;

    void Start()
    {
        honeyCollectionTimeLeft = honeyCollectionTime;
        slider = progressBar.GetComponent<Slider>();
        slider.maxValue = honeyCollectionTime;
        subtitles = GetComponent<Subtitles>();
        audioPlayer = GetComponentInParent<AudioScript>();
    }

    void FixedUpdate()
    {
        // Bit shift the index of the layer (9) to get a bit mask
        int layerMask = 1 << 9;

        RaycastHit hit;

        if(honeyCollectionTimeLeft > 0)
        { 
            // Does the ray intersect any objects excluding the beehive layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactionDistance, layerMask))
            {
                progressBar.SetActive(true);
                textElement.SetActive(true);
                if (Input.GetKey(KeyCode.E))
                {
                    honeyCollectionTimeLeft -= Time.fixedDeltaTime;
                    slider.value = honeyCollectionTime - honeyCollectionTimeLeft;
                }
            }
            else
            {
                progressBar.SetActive(false);
                textElement.SetActive(false);
            }
        }
        else     //What happens when honey is collected
        {
            if (!taskCompleted)
            {
                playerMovementRecorder.endTaskCompleted = true;
                taskCompleted = true;
                audioPlayer.PlayAudioClip(endTaskClip, false);
                audioPlayer.PlayAudioClip(returnMusicClip, true, 0.45f);
                audioPlayer.PlayEffect(returnEffect, 0.12f);
                subtitles.SetupUpSubtitles(subtitleText, subtitleTime);
                subtitles.Play();
                ChangeAudioTriggerActiveness();
                textElement.SetActive(false);
            }
            progressBar.SetActive(false);
            endTestTrigger.SetActive(true);
            mistakeTriggerParent.SetActive(true);
            shortcutTriggerParent.SetActive(true);
        }
    }

    private void ChangeAudioTriggerActiveness()
    {
        foreach (Transform triggerParent in audioTriggers.transform)
        {
            foreach (Transform trigger in triggerParent.transform)
                trigger.GetComponent<AudioTrigger>().ChangeActiveness();
        }
    }
}

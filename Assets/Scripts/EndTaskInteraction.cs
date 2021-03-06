﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndTaskInteraction : MonoBehaviour
{
    public GameObject angryBee;
    public GameObject beeSpawnPoint;
    [SerializeField] private int beeSpawnAmount = 3;
    [SerializeField] private int beeSpawnDelay = 1;
    private bool hasAngryBeeSpawned = false;
    
    [SerializeField] private float honeyCollectionTime = 10;
    private float honeyCollectionTimeLeft;

    [SerializeField] private GameObject interactionBackdrop;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject textElement;
    [SerializeField] private PlayerMovementRecorder playerMovementRecorder;

    [SerializeField] private float interactionDistance;

    private Slider slider;
    private bool taskCompleted = false;

    [SerializeField] private NavigationalAid beeNavAid;
    [SerializeField] private NavigationalAid arrowNavAid;

    [SerializeField] private GameObject endTestTrigger;
    [SerializeField] private GameObject mistakeTriggerParent;
    [SerializeField] private GameObject shortcutTriggerParent;

    [SerializeField] private GameObject audioTriggers;

    [SerializeField] private AudioClip returnMusicClip;
    [SerializeField] private AudioClip returnEffect;

    [SerializeField] private AudioClip endTaskClip;
    [SerializeField] private int[] subtitleBackdropSize;
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
                interactionBackdrop.GetComponent<Image>().enabled = true;
                textElement.GetComponent<Text>().text = "Hold E to collect honey";
                progressBar.SetActive(true);
                textElement.SetActive(true);
                slider.maxValue = honeyCollectionTime;
                if (Input.GetKey(KeyCode.E))
                {
                    honeyCollectionTimeLeft -= Time.fixedDeltaTime;
                    slider.value = honeyCollectionTime - honeyCollectionTimeLeft;
                }
            }
            else
            {
                interactionBackdrop.GetComponent<Image>().enabled = false;
                progressBar.SetActive(false);
                textElement.SetActive(false);
            }
        }
        else     //What happens when honey is collected
        {
            if (!taskCompleted)
            {
                taskCompleted = true;
                if(beeNavAid != null)
                    beeNavAid.DisableAidWithSound();
                if (arrowNavAid != null)
                    arrowNavAid.DisableAidWithSound();
                playerMovementRecorder.endTaskCompleted = true;
                audioPlayer.PlayAudioClip(returnMusicClip, true, 0.45f);
                audioPlayer.PlayEffect(returnEffect, true, 0.12f);
                audioPlayer.PlayAudioClip(endTaskClip, false);
                interactionBackdrop.GetComponent<Image>().enabled = false;
                subtitles.SetupUpSubtitles(subtitleText, subtitleTime, subtitleBackdropSize);
                subtitles.Play();
                ChangeAudioTriggerActiveness();
                textElement.SetActive(false);
            }
            progressBar.SetActive(false);
            endTestTrigger.SetActive(true);
            mistakeTriggerParent.SetActive(true);
            shortcutTriggerParent.SetActive(true);

            if (!hasAngryBeeSpawned)
            {
                hasAngryBeeSpawned = true;

                StartCoroutine(BeeSpawn());
            }
        }
    }

    IEnumerator BeeSpawn()
    {
        //Time damage effect delay to when bee stings
        
        for (int i = 0; i < beeSpawnAmount; i++)
            {
                //Spawn bee at position and set player for targetting to be the player object
                GameObject bee = Instantiate(angryBee, beeSpawnPoint.transform.position, Quaternion.identity);
                bee.GetComponent<BeeAngryController>().player = gameObject.transform.parent.gameObject.transform;

                //apply the bee object to the endtest script in the parent object, so we can remove when we reach the cave
                gameObject.transform.parent.gameObject.GetComponent<EndTest>().angryBeeList.Add(bee);
                yield return new WaitForSeconds(beeSpawnDelay);
            }

        //yield return null;
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTest : MonoBehaviour
{
    [System.NonSerialized] public bool endTaskCompleted = false;

    [SerializeField] private GameObject textElement;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EndTestTrigger>() != null)
        {
            //End the recording
            transform.GetComponent<PlayerMovementRecorder>().testEnded = true;
            textElement.SetActive(true);
            textElement.GetComponent<Text>().text = "You have saved your cubs! You can now quit the game by pressing ALT + F4 and fill out the questionnaire.";
        }
    }
}

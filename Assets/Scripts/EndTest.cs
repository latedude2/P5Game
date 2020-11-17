using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTest : MonoBehaviour
{
    [System.NonSerialized] public bool endTaskCompleted = false;
    [System.NonSerialized] public string endingText = "You can now quit the game by pressing ALT + F4 and fill out the questionnaire.";

    [SerializeField] private GameObject textElement;

    public GameObject angryBee;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EndTestTrigger>() != null)
        {
            //End the recording
            transform.GetComponent<PlayerMovementRecorder>().testEnded = true;
            textElement.SetActive(true);
            textElement.GetComponent<Text>().fontSize = 25;
            textElement.GetComponent<Text>().text = endingText;

            //Remove the chasing bee
            Destroy(angryBee);
        }
    }
}

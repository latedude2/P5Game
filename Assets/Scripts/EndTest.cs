using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTest : MonoBehaviour
{
    [System.NonSerialized] public bool endTaskCompleted = false;
    [System.NonSerialized] public string endingText = "You can now quit the game by pressing ALT + F4.";

    [SerializeField] private GameObject interactionBackdrop;
    [SerializeField] private int textBackdropWidth;
    [SerializeField] private int textBackdropHeight;
    [SerializeField] private GameObject textElement;

    public List<GameObject> angryBeeList;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EndTestTrigger>() != null)
        {
            //End the recording
            //transform.GetComponent<PlayerMovementRecorder>().testEnded = true; removed for itch release
            textElement.SetActive(true);

            interactionBackdrop.GetComponent<Image>().enabled = true;
            interactionBackdrop.GetComponent<RectTransform>().sizeDelta = new Vector2(textBackdropWidth, textBackdropHeight);
            textElement.GetComponent<Text>().fontSize = 25;
            textElement.GetComponent<Text>().text = endingText;

            //Remove the chasing bees
            foreach (GameObject angryBee in angryBeeList)
            {
                Destroy(angryBee);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CobwebInteraction : MonoBehaviour
{
    [SerializeField] private float interactionTime = 3;
    private float interactionTimeLeft;

    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject textElement;

    [SerializeField] private float interactionDistance;
    private GameObject cobweb;

    private Slider slider;
    private bool taskCompleted = false;

    //[SerializeField] private AudioClip endTaskClip;
    //private AudioScript audioPlayer;

    void Start()
    {
        interactionTimeLeft = interactionTime;
        slider = progressBar.GetComponent<Slider>();
        slider.maxValue = interactionTime;
        //audioPlayer = GetComponentInParent<AudioScript>();
    }

    void FixedUpdate()
    {
        // Bit shift the index of the layer (10) to get a bit mask
        int layerMask = 1 << 10;

        RaycastHit hit;

        if(interactionTimeLeft > 0)
        { 
            // Does the ray intersect any objects excluding the beehive layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactionDistance, layerMask))
            {
                textElement.GetComponent<Text>().text = "Press E to break cobweb";
                cobweb = hit.transform.gameObject;
                progressBar.SetActive(true);
                textElement.SetActive(true);
                if (Input.GetKey(KeyCode.E))
                {
                    interactionTimeLeft -= Time.fixedDeltaTime;
                    slider.value = interactionTime - interactionTimeLeft;
                }
            }
            else
            {
                progressBar.SetActive(false);
                textElement.SetActive(false);
            }
        }
        else     //What happens when cobweb is destroyed
        {
            if (!taskCompleted)
            {
                taskCompleted = true;
                cobweb.SetActive(false);
                //audioPlayer.PlayAudioClip(endTaskClip, false);
                //audioPlayer.PlayAudioClip(returnMusicClip, true, 0.45f);
                this.gameObject.GetComponent<CobwebInteraction>().enabled = false;
                this.gameObject.GetComponent<EndTaskInteraction>().enabled = true;
                slider.value = 0;
                textElement.SetActive(false);
                
            }
            progressBar.SetActive(false);
            
        }
    }
}

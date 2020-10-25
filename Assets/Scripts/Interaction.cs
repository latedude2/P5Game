using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Interaction : MonoBehaviour
{
    
    [SerializeField] private float honeyCollectionTime = 10;
    private float honeyCollectionTimeLeft;

    private float taskCompletionTextShowTime = 5;

    [SerializeField] private GameObject progressBar;
    [SerializeField] private GameObject textElement;

    [SerializeField] private float interactionDistance;

    private Slider slider;
    private Text text;


    // Start is called before the first frame update
    void Start()
    {
        honeyCollectionTimeLeft = honeyCollectionTime;
        slider = progressBar.GetComponent<Slider>();
        text = textElement.GetComponent<Text>();
        slider.maxValue = honeyCollectionTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 9;

        RaycastHit hit;

        if(honeyCollectionTimeLeft > 0)
        { 
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3, layerMask))
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
            progressBar.SetActive(false);
            taskCompletionTextShowTime -= Time.fixedDeltaTime;
            text.text = "Honey has been collected and the bees are angry! Return home!";

            if (taskCompletionTextShowTime < 0)
                textElement.SetActive(false);
        }
            
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationalAid : MonoBehaviour
{
    public void DisableAidWithSound()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        if(GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().enabled = false;
    }
}

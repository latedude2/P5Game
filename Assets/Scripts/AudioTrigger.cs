using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public bool isReturnTrigger = false;
    public bool stopEffect = false;

    public AudioClip triggerClip;
    public bool isMusicTrigger = false;
    public bool destroyOnPlay = true;

    public bool changeMusic = false;
    public AudioClip musicClip;

    public int[] subtitleBackdropSize;
    public string[] subtitleText;
    public float[] subtitleTime;

    private GameObject audioTriggerGroup;

    private void Start()
    {
        audioTriggerGroup = transform.parent.gameObject;

        if (isReturnTrigger)
            // deactivate the AudioTrigger group, which works when returning
            audioTriggerGroup.SetActive(false);
    }

    public void ChangeActiveness()
    {
        // change the activeness of the AudioTrigger group
        audioTriggerGroup.SetActive(!audioTriggerGroup.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public bool isReturnTrigger = false;

    public AudioClip triggerClip;
    public bool isMusicTrigger = false;
    public bool destroyOnPlay = true;

    public string[] subtitleText;
    public float[] subtitleTime;

    private void Start()
    {
        if (isReturnTrigger)
            transform.parent.gameObject.SetActive(false);
    }

    public void ChangeActiveness()
    {
        transform.parent.gameObject.SetActive(!transform.parent.gameObject.activeSelf);
    }
}

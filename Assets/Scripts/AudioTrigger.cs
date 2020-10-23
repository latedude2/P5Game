using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioClip triggerClip;
    public bool isMusicTrigger = false;
    public bool destroyOnPlay = true;

    public bool playSubtitles = true;
    public string[] subtitleText;
    public float[] subtitleTime;
}

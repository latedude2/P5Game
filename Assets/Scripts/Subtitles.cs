using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Subtitles : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    private string[] subtitleText;
    private float[] subtitleTime;

    private bool isNarrativePlaying = false;
    private float startTime;
    private int subtitleCounter = 0;

    void FixedUpdate()
    {
        if (isNarrativePlaying)
        {
            if (IsSubtitleOver(subtitleTime[subtitleCounter]))
            {
                subtitleCounter++;
                SetTime();
            }

            if (subtitleCounter >= subtitleText.Length)
            {
                isNarrativePlaying = false;
                textMesh.text = " ";
            }
            else
            {
                textMesh.text = subtitleText[subtitleCounter];
            }
        }
    }

    public void SetupUpSubtitles(string[] text, float[] time)
    {
        subtitleText = text;
        subtitleTime = time;
        subtitleCounter = 0;
    }

    private void SetTime()
    {
        startTime = Time.time;
    }

    private bool IsSubtitleOver(float subtitleTime)
    {
        return startTime + subtitleTime <= Time.time;
    }

    public void Play()
    {
        SetTime();
        isNarrativePlaying = true;
    }
}

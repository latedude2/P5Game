using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Subtitles : MonoBehaviour
{
    [SerializeField] private GameObject backdrop;
    [SerializeField] private TextMeshProUGUI textMesh;

    [SerializeField] private int subtitleBackdropHeight = 40;
    private int[] subtitleBackdropWidth;
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
                backdrop.GetComponent<Image>().enabled = false;
                isNarrativePlaying = false;
                textMesh.text = " ";
            }
            else
            {
                backdrop.GetComponent<Image>().enabled = true;
                backdrop.GetComponent<RectTransform>().sizeDelta = new Vector2(subtitleBackdropWidth[subtitleCounter], subtitleBackdropHeight);
                textMesh.text = subtitleText[subtitleCounter];
            }
        }
    }

    public void SetupUpSubtitles(string[] text, float[] time, int[] backdropSize)
    {
        subtitleBackdropWidth = backdropSize;
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

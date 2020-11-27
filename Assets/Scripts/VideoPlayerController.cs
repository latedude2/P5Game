using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject mainGameObject;

    private VideoPlayer videoPlayer;
    private long playerFrameCount;
    void Start()
    {
        mainGameObject.SetActive(false);
        Debug.developerConsoleVisible = true;
        videoPlayer = GetComponent<VideoPlayer>();
        playerFrameCount = Convert.ToInt64(videoPlayer.frameCount);
        videoPlayer.Play();

        //invoke method every .1f second from 0f time
        InvokeRepeating("CheckOver", 0f, .1f);
    }

    private void CheckOver()
    {
        long playerCurrentFrame = videoPlayer.frame;

        if (playerCurrentFrame >= playerFrameCount)
        {
            gameObject.SetActive(false);
            videoPlayer.Stop();
            mainGameObject.SetActive(true);
            //Cancel Invoke since video is no longer playing
            CancelInvoke("CheckOver");
        }
    }
}

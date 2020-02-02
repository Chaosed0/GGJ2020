﻿using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Diagnostics;

public class WinCollider : MonoBehaviour
{
    private const string videoLocation = "Data/Video.mp4";
    private const string crashDataLocation = "crash.log";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerCollider = collision.GetComponent<PlayerCollider>();
        if (playerCollider != null)
        {
            Win();
        }
    }

    private void Win()
    {
        var videoPath = MetaLoadUtil.GetPath(videoLocation);
        if (!File.Exists(videoPath))
        {
            //tinyfd.tinyfd_beep();
            //tinyfd.tinyfd_messageBox("Error!", "Error!", "ok", "error", 1);

            var message = $"Victory video not found in {videoPath}! (TODD SAYS TO GET IT FROM https://chaosed0.github.io/GGJ2020/TestEnd.video)";
            using (var writer = new StreamWriter(crashDataLocation, false))
            {
                writer.Write(message);
            }

            if (!Application.isEditor)
            {
                Debug.LogError(message);
                Utils.ForceCrash(ForcedCrashCategory.Abort);
            }
            else
            {
                Debug.Log("Crash!");
            }

            return;
        }

        var videoPlayer = Camera.main.GetComponent<VideoPlayer>();
        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }
}

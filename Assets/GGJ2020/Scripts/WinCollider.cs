using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class WinCollider : MonoBehaviour
{
    private const string videoLocation = "Data/Video.mp4";

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
            if (!Application.isEditor)
            {
                Application.Quit();
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

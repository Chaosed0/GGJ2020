using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Diagnostics;

public class WinCollider : MonoBehaviour
{
    private const string videoLocation = "Assets/Video.mp4";
    private const string crashDataLocation = "crash.log";

    private bool won = false;

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
        if (won)
        {
            return;
        }

        var videoPath = MetaLoadUtil.GetPath(videoLocation);
        var fileInfo = new FileInfo(videoPath);
        if (!fileInfo.Exists)
        {
            var message = $"Victory video not found in {videoPath}! (TODD SAYS TO GET IT FROM https://chaosed0.github.io/GGJ2020/TestEnd.video)";

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            tinyfd.tinyfd_beep();
            tinyfd.tinyfd_messageBox("Error!", "Consult error.log for more details", "ok", "error", 1);
#endif

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
                Debug.Log(message);
            }
        }
        else
        {
            var videoPlayer = Camera.main.GetComponent<VideoPlayer>();
            videoPlayer.url = fileInfo.FullName;
            videoPlayer.Play();

            won = true;
        }
    }
}

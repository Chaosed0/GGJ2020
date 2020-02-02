using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using System.IO;
using UnityEngine.Diagnostics;

public class WinCollider : MonoBehaviour
{
    public UnityAction OnGameCrashed = null;
    public UnityAction OnVictory = null;

    private const string videoLocation = "Assets/Victory.video";
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
            var message = $"Victory video not found in {videoPath}! (TODD SAYS TO GET IT FROM https://chaosed0.github.io/GGJ2020/Victory.video)";

            OnGameCrashed?.Invoke();

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            Util.OnNextFrame(this, () => tinyfd.tinyfd_beep());
            Util.OnNextFrame(this, () => tinyfd.tinyfd_messageBox("Error!", "Consult crash.log for more details", "ok", "error", 1));
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
            videoPlayer.Play();

            won = true;

            OnVictory?.Invoke();
        }
    }
}

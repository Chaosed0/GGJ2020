using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public AudioSource narratorAudioSource;

    public float introDelay = 1.0f;

    public AudioClip introClip;

    private Player player;
    private MovePlayer movePlayer;
    private Status playerStatus;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player != null)
        {
            player.OnPositionChanged += HandlePositionChanged;
            player.OnPositionRejected += HandlePositionRejected;

            movePlayer = player.GetComponent<MovePlayer>();

            playerStatus = player.GetComponent<Status>();
            if (playerStatus != null)
            {
                playerStatus.OnDead += HandlePlayerDeath;
            }
        }

        // Increment the number of times the game has been launched
        PlayerPrefs.SetInt("NumLaunches", PlayerPrefs.GetInt("NumLaunches", 0) + 1);
        // Debug.Log($"Number of Launches: {PlayerPrefs.GetInt("NumLaunches", 0)}");

        Util.ExecuteAfterTime(this, introDelay, PotentiallyPlayIntro);
    }

    private void Update()
    {
        // Hold down Tilde, then press and release the C button to clear PlayerPrefs
        if (Input.GetKey(KeyCode.BackQuote))
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                // Debug.Log("Clearing Player Prefs...");
                PlayerPrefs.DeleteAll();
            }
        }
    }

    private void PotentiallyPlayIntro()
    {
        if (PlayerPrefs.GetInt("NumLaunches", 0) <= 1)
        {
            // Debug.Log("Playing Intro (if available)");
            if (introClip != null)
            {
                narratorAudioSource.clip = introClip;
                narratorAudioSource.Play();
            }
        }
    }

    private void HandlePositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {

    }

    private void HandlePositionRejected(Vector3 oldPosition, Vector3 newPosition)
    {

    }

    private void HandlePlayerDeath()
    {

    }
}

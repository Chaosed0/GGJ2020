using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public AudioSource narratorAudioSource;

    public float introDelay = 1.0f;

    public AudioClip introClip;
    public AudioClip keyBindClip;
    public AudioClip tileSetClip;

    private Player player;
    private MovePlayer movePlayer;
    private Status playerStatus;

    private bool keysCorrectlyBound;
    private bool keyBindingPromptPlayed;
    private bool tileSetPromptPlayed;

    private int upAttempts;
    private int leftAttempts;
    private int rightAttempts;
    private int downAttempts;

    private int walkThruWallAttempts;

    private bool logging = true;

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
        if (logging) Debug.Log($"Number of Launches: {PlayerPrefs.GetInt("NumLaunches", 0)}");

        Util.ExecuteAfterTime(this, 0.02f, CheckPuzzleSolutions);
        Util.ExecuteAfterTime(this, 0.02f + introDelay, PotentiallyPlayIntro);
    }

    private void Update()
    {
        if (keysCorrectlyBound == false && keyBindingPromptPlayed == false)
        {
            CountMovementAttempts();

            if (upAttempts + downAttempts >= 1)
            {
                Util.ExecuteAfterTime(this, 2.0f, PlayKeyBindingsPrompt);
                keyBindingPromptPlayed = true;
            }
        }
        
        // Hold down Tilde, then press and release the C button to clear PlayerPrefs
        if (Input.GetKey(KeyCode.BackQuote))
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                if (logging) Debug.Log("Clearing Player Prefs...");
                PlayerPrefs.DeleteAll();
            }
        }
    }

    private void CheckPuzzleSolutions()
    {
        if (movePlayer.AreKeysCorrectlyBound())
        {
            PlayerPrefs.SetInt("KeyBindsFixed", PlayerPrefs.GetInt("KeyBindsFixed", 0) + 1);
            keysCorrectlyBound = true;
        }
    }

    private void PotentiallyPlayIntro()
    {
        if (PlayerPrefs.GetInt("NumLaunches", 0) <= 1)
        {
            if (logging) Debug.Log("Playing Intro (if available)");
            if (introClip != null)
            {
                narratorAudioSource.clip = introClip;
                narratorAudioSource.Play();
            }
        }
        /*
        else if (PlayerPrefs.GetInt("KeyBindsFixed", 0) <= 0)
        {

        }
        */
    }

    private void PlayKeyBindingsPrompt()
    {
        if (logging) Debug.Log("NarrationManager::PlayKeyBindingsPrompt()");
        if (keyBindClip != null)
        {
            narratorAudioSource.clip = keyBindClip;
            narratorAudioSource.Play();
        }
    }

    private void PlayTileSetPrompt()
    {
        if (logging) Debug.Log("NarrationManager::PlayTileSetPrompt()");
        if (tileSetClip != null)
        {
            narratorAudioSource.clip = tileSetClip;
            narratorAudioSource.Play();
        }
    }

    private void HandlePositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {

    }

    private void HandlePositionRejected(Vector3 oldPosition, Vector3 newPosition, Collider2D hitCollider)
    {
        if (keysCorrectlyBound == true)
        {
            if (tileSetPromptPlayed == false)
            {
                if (LoadSpriteFromDisk.IsTransparent("Assets/Images/Wall.png") == true)
                {
                    walkThruWallAttempts++;
                    if (logging) Debug.Log($"Walk Through Wall Attempts: {walkThruWallAttempts}");

                    if (walkThruWallAttempts >= 3)
                    {
                        Util.ExecuteAfterTime(this, 1.0f, PlayTileSetPrompt);
                        tileSetPromptPlayed = true;
                    }
                }
            }
        }
    }

    private void HandlePlayerDeath()
    {

    }

    private void CountMovementAttempts()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (logging) Debug.Log("Up Attempt");
            upAttempts++;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (logging) Debug.Log("Left Attempt");
            leftAttempts++;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (logging) Debug.Log("Down Attempt");
            downAttempts++;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (logging) Debug.Log("Right Attempt");
            rightAttempts++;
        }
    }
}

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class NarrationManager : MonoBehaviour
{
    public AudioSource narratorAudioSource;

    [FormerlySerializedAs("introDelay")]
    public float narratorDelay = 1.0f;

    public AudioClip introClip;
    [FormerlySerializedAs("keyBindClip")]
    public AudioClip keyBindsBrokenClip;
    [FormerlySerializedAs("tileSetClip")]
    public AudioClip tileSetBrokenClip;
    public AudioClip entityStatsBrokenClip;
    public AudioClip gameCrashClip;
    public AudioClip victoryClip;

    private Player _player;
    private Player player
    {
        get
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            }
            return _player;
        }
    }

    private MovePlayer _movePlayer;
    private MovePlayer movePlayer
    {
        get
        {
            if (_movePlayer == null)
            {
                _movePlayer = player.GetComponent<MovePlayer>();
            }
            return _movePlayer;
        }
    }

    private Status _playerStatus;
    private Status playerStatus
    {
        get
        {
            if (_playerStatus == null)
            {
                _playerStatus = player.GetComponent<Status>();
            }
            return _playerStatus;
        }
    }

    private Status _enemyStatus;
    private Status enemyStatus
    {
        get
        {
            if (_enemyStatus == null)
            {
                _enemyStatus = GameObject.FindGameObjectWithTag("Enemy")?.GetComponent<Status>();
            }
            return _enemyStatus;
        }
    }

    private WinCollider _flag;
    private WinCollider flag
    {
        get
        {
            if (_flag == null)
            {
                _flag = GameObject.FindGameObjectWithTag("Flag")?.GetComponentInChildren<WinCollider>();
            }
            return _flag;
        }
    }

    private bool keyBindingsFixed;
    private bool keyBindingPromptPlayed;
    private bool tileSetFixed;
    private bool tileSetPromptPlayed;
    private bool entityStatsFixed;
    private bool entityStatsPromptPlayed;

    private int upAttempts;
    private int leftAttempts;
    private int rightAttempts;
    private int downAttempts;

    private int walkThruWallAttempts;

    private bool logging = true;

    private void Start()
    {
        SubscribeToEvents();

        // Increment the number of times the game has been launched
        PlayerPrefs.SetInt("NumLaunches", PlayerPrefs.GetInt("NumLaunches", 0) + 1);
        if (logging) Debug.Log($"Number of Launches: {PlayerPrefs.GetInt("NumLaunches", 0)}");

        Util.ExecuteAfterTime(this, 0.1f, CheckPuzzleSolutions);
        Util.ExecuteAfterTime(this, 0.1f + narratorDelay, PotentiallyPlayIntro);

        // We only have one level, so no need to specify which one
        SceneManager.sceneLoaded += (x, y) => { SubscribeToEvents(); };
    }

    private void SubscribeToEvents()
    {
        if (player != null)
        {
            player.OnPositionRejected += HandlePositionRejected;

            if (playerStatus != null)
            {
                playerStatus.OnDead += HandlePlayerDeath;
            }
        }

        if (flag != null)
        {
            flag.OnGameCrashed += HandleGameCrashed;
            flag.OnVictory += HandleVictory;
        }
    }

    private void Update()
    {
        if (keyBindingsFixed == false && keyBindingPromptPlayed == false)
        {
            CountMovementAttempts();

            if (upAttempts + downAttempts >= 1)
            {
                // Leave this check in if we want the intro to continue playing before the key bind prompt plays
                if (narratorAudioSource.isPlaying == false)
                {
                    Util.ExecuteAfterTime(this, narratorDelay, PlayKeyBindingsBroken);
                    keyBindingPromptPlayed = true;
                }
            }
        }
        
        // Hold down Tilde, then press and release the C button to clear PlayerPrefs
        if (Input.GetKey("`"))
        {
            if (Input.GetKeyUp("c"))
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
            if (logging) Debug.Log("Key Binds Fixed");
            keyBindingsFixed = true;
        }

        if (LoadSpriteFromDisk.IsTransparent("Assets/Images/Wall.png") == false)
        {
            PlayerPrefs.SetInt("TileSetFixed", PlayerPrefs.GetInt("TileSetFixed", 0) + 1);
            if (logging) Debug.Log("Tile Set Fixed");
            tileSetFixed = true;
        }

        if ((enemyStatus.attack == 0 ? int.MaxValue : (playerStatus.health / enemyStatus.attack)) >= (playerStatus.attack == 0 ? int.MinValue : (enemyStatus.health / playerStatus.attack)))
        {
            PlayerPrefs.SetInt("EntityStatsFixed", PlayerPrefs.GetInt("EntityStatsFixed", 0) + 1);
            if (logging) Debug.Log("Entity Stats Fixed");
            entityStatsFixed = true;
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

    private void PlayKeyBindingsBroken()
    {
        if (logging) Debug.Log("NarrationManager::PlayKeyBindingsPrompt()");
        if (keyBindsBrokenClip != null)
        {
            narratorAudioSource.clip = keyBindsBrokenClip;
            narratorAudioSource.Play();
        }
    }

    private void PlayTileSetBroken()
    {
        if (logging) Debug.Log("NarrationManager::PlayTileSetPrompt()");
        if (tileSetBrokenClip != null)
        {
            narratorAudioSource.clip = tileSetBrokenClip;
            narratorAudioSource.Play();
        }
    }

    private void PlayEntityStatsBroken()
    {
        if (logging) Debug.Log("NarrationManager::PlayEntityStatsBroken()");
        if (entityStatsBrokenClip != null)
        {
            narratorAudioSource.clip = entityStatsBrokenClip;
            narratorAudioSource.Play();
        }
    }

    private void PlayGameCrashed()
    {
        if (gameCrashClip != null)
        {
            narratorAudioSource.clip = gameCrashClip;
            narratorAudioSource.Play();
        }
    }

    private void PlayVictory()
    {
        if (victoryClip != null)
        {
            narratorAudioSource.clip = victoryClip;
            narratorAudioSource.Play();
        }
    }

    private void HandleGameCrashed()
    {
        PlayGameCrashed();
    }

    private void HandleVictory()
    {
        PlayVictory();
    }

    private void HandlePositionRejected(Vector3 oldPosition, Vector3 newPosition, Collider2D hitCollider)
    {
        if (logging) Debug.Log($"NarrationManager::HandlePositionRejected()");
        if (keyBindingsFixed == true)
        {
            if (logging) Debug.Log($"keyBindingsFixed: {keyBindingsFixed}");
            if (logging) Debug.Log($"tileSetFixed: {tileSetFixed}");
            if (logging) Debug.Log($"tileSetPromptPlayed: {tileSetPromptPlayed}");

            if (tileSetFixed == false && tileSetPromptPlayed == false)
            {
                walkThruWallAttempts++;
                if (logging) Debug.Log($"Walk Through Wall Attempts: {walkThruWallAttempts}");

                if (narratorAudioSource.isPlaying == false && walkThruWallAttempts >= 3)
                {
                    Util.ExecuteAfterTime(this, 1.0f, PlayTileSetBroken);
                    tileSetPromptPlayed = true;
                }
            }
        }
    }

    private void HandlePlayerDeath()
    {
        if (logging) Debug.Log("NarrationManager::HandlePlayerDeath()");

        if (logging) Debug.Log($"keyBindingsFixed: {keyBindingsFixed}");
        if (logging) Debug.Log($"tileSetFixed: {tileSetFixed}");

        if (keyBindingsFixed && tileSetFixed)
        {
            if (entityStatsFixed)
            {
                if (entityStatsPromptPlayed == false)
                {
                    Util.ExecuteAfterTime(this, narratorDelay, PlayEntityStatsBroken);
                    entityStatsPromptPlayed = true;
                }
            }
        }
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

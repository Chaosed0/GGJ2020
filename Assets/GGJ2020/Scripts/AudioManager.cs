using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _Instance;
    public static AudioManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<AudioManager>();
            }
            return _Instance;
        }
    }

    [FormerlySerializedAs("playerMoveAudioSource")]
    public AudioSource playerActionAudioSource;

    public AudioClip[] wallHit;
    public AudioClip[] playerMoveShort;
    public AudioClip[] playerMoveLong;
    public AudioClip[] playerDeath;

    private Player player;
    private Status playerStatus;

    private System.Random rand;

    private void Awake()
    {
        rand = new System.Random();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player != null)
        {
            player.OnPositionChanged += HandlePositionChanged;
            player.OnPositionRejected += HandlePositionRejected;

            playerStatus = player.GetComponent<Status>();
            if (playerStatus != null)
            {
                playerStatus.OnDead += HandlePlayerDeath;
            }
        }
    }

    private void HandlePositionRejected(Vector3 oldPosition, Vector3 newPosition)
    {
        if (wallHit != null && wallHit.Length > 0)
        {
            int index = rand.Next(wallHit.Length);
            playerActionAudioSource.clip = wallHit[index];
            playerActionAudioSource.Play();
        }
    }

    private void HandlePositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        if (playerMoveLong != null && playerMoveLong.Length > 0)
        {
            int index = rand.Next(playerMoveLong.Length);
            playerActionAudioSource.clip = playerMoveLong[index];
            playerActionAudioSource.Play();
        }
    }

    private void HandlePlayerDeath()
    {
        if (playerDeath != null && playerDeath.Length > 0)
        {
            int index = rand.Next(playerDeath.Length);
            playerActionAudioSource.clip = playerDeath[index];
            playerActionAudioSource.Play();
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnPositionChanged -= HandlePositionChanged;
            player.OnPositionRejected -= HandlePositionRejected;

            playerStatus = player.GetComponent<Status>();
            if (playerStatus != null)
            {
                playerStatus.OnDead -= HandlePlayerDeath;
            }
        }
    }
}

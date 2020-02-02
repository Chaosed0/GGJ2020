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

    public AudioSource enemyActionAudioSource;

    public AudioClip[] wallHit;
    public AudioClip[] playerMoveShort;
    public AudioClip[] playerMoveLong;
    public AudioClip[] playerDeath;
    public AudioClip[] playerAttack;
    public AudioClip[] enemyMove;
    public AudioClip[] enemyDeath;

    private Player player;
    private Player enemy;
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

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Player>();
        if (enemy != null)
        {
            enemy.OnPositionChanged += HandleEnemyPositionChanged;
            var enemyStatus = enemy.GetComponent<Status>();
            if (enemyStatus != null)
            {
                enemyStatus.OnDead += HandleEnemyDeath;
            }
        }
    }

    private void HandlePositionRejected(Vector3 oldPosition, Vector3 newPosition, Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayRandom(playerActionAudioSource, playerAttack);
        }
        else
        {
            PlayRandom(playerActionAudioSource, wallHit);
        }
    }

    private void HandlePositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        PlayRandom(playerActionAudioSource, playerMoveLong);
    }

    private void HandleEnemyPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        PlayRandom(enemyActionAudioSource, enemyMove);
    }

    private void HandleEnemyDeath()
    {
        PlayRandom(enemyActionAudioSource, enemyDeath);
    }

    private void HandlePlayerDeath()
    {
        PlayRandom(playerActionAudioSource, playerDeath);
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

    private void PlayRandom(AudioSource source, AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            int index = rand.Next(clips.Length);
            source.clip = clips[index];
            source.Play();
        }
    }
}

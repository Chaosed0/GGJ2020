using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

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

    private Player _player;
    private Player player
    {
        get
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            }
            return _player;
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

    private Player _enemy;
    private Player enemy
    {
        get
        {
            if (_enemy == null)
            {
                _enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Player>();
            }
            return _enemy;
        }
    }

    private System.Random rand;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        rand = new System.Random();

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (player != null)
        {
            player.OnPositionChanged += HandlePositionChanged;
            player.OnPositionRejected += HandlePositionRejected;

            if (playerStatus != null)
            {
                playerStatus.OnDead += HandlePlayerDeath;
            }
        }

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
        PlayRandom(playerActionAudioSource, playerMoveLong, 0.3f);
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

            if (playerStatus != null)
            {
                playerStatus.OnDead -= HandlePlayerDeath;
            }
        }
    }

    private void PlayRandom(AudioSource source, AudioClip[] clips, float volume = 1f)
    {
        if (clips != null && clips.Length > 0)
        {
            int index = rand.Next(clips.Length);
            source.clip = clips[index];
            source.volume = volume;
            source.Play();
        }
    }
}

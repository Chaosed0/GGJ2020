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

    public Player player;

    private Status playerStatus;

    [FormerlySerializedAs("playerMoveAudioSource")]
    public AudioSource playerActionAudioSource;

    public AudioClip[] playerMoveShort;
    public AudioClip[] playerMoveLong;
    public AudioClip[] playerDeath;

    private System.Random rand;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        rand = new System.Random();
    }

    private void Start()
    {
        if (player != null)
        {
            player.OnPositionChanged += (x, y) => { PlayPlayerMoveLong(); };
            player.OnPositionRejected += (x, y) => { PlayPlayerMoveShort(); };

            playerStatus = GetComponent<Status>();
            if (playerStatus != null)
            {
                playerStatus.OnDead += PlayPlayerDeath;
            }
        }
    }

    private void PlayPlayerMoveShort()
    {
        if (playerMoveShort != null && playerMoveShort.Length > 0)
        {
            int index = rand.Next(playerMoveShort.Length);
            playerActionAudioSource.clip = playerMoveShort[index];
            playerActionAudioSource.Play();
        }
    }

    private void PlayPlayerMoveLong()
    {
        if (playerMoveLong != null && playerMoveLong.Length > 0)
        {
            int index = rand.Next(playerMoveLong.Length);
            playerActionAudioSource.clip = playerMoveLong[index];
            playerActionAudioSource.Play();
        }
    }

    private void PlayPlayerDeath()
    {
        if (playerDeath != null && playerDeath.Length > 0)
        {
            int index = rand.Next(playerDeath.Length);
            playerActionAudioSource.clip = playerDeath[index];
            playerActionAudioSource.Play();
        }
    }
}

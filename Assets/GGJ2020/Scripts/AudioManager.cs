using UnityEngine;

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

    public AudioSource playerMoveAudioSource;

    public AudioClip[] playerMoveShort;
    public AudioClip[] playerMoveLong;

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
        }
    }

    public void PlayPlayerMoveShort()
    {
        if (playerMoveShort != null && playerMoveShort.Length > 0)
        {
            int index = rand.Next(playerMoveShort.Length);
            playerMoveAudioSource.clip = playerMoveShort[index];
            playerMoveAudioSource.Play();
        }
    }

    public void PlayPlayerMoveLong()
    {
        if (playerMoveLong != null && playerMoveLong.Length > 0)
        {
            int index = rand.Next(playerMoveLong.Length);
            playerMoveAudioSource.clip = playerMoveLong[index];
            playerMoveAudioSource.Play();
        }
    }
}

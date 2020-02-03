using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject youDied = null;

    private Status playerStatus = null;

    private void Start()
    {
        Debug.Log($"Is wall transparent: {LoadSpriteFromDisk.IsTransparent("Assets/Images/Wall.png")}");

        youDied.GetComponent<Canvas>().enabled = false;
        youDied.GetComponent<Animator>().enabled = false;
        var player = GameObject.FindGameObjectWithTag("Player");
        this.playerStatus = player.GetComponent<Status>();

        playerStatus.OnDead += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        StartCoroutine(YouDiedCoroutine());
    }

    private IEnumerator YouDiedCoroutine()
    {
        youDied.GetComponent<Canvas>().enabled = true;
        youDied.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}

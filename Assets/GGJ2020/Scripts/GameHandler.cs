using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private Status playerStatus = null;

    [SerializeField]
    private GameObject youDied = null;

    private void Start()
    {
        youDied.SetActive(false);
        playerStatus.OnDead += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        StartCoroutine(YouDiedCoroutine());
    }

    private IEnumerator YouDiedCoroutine()
    {
        youDied.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}

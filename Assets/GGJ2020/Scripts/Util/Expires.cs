using UnityEngine;
using System.Collections;
using Obelus;

public class Expires : MonoBehaviour
{
    [SerializeField]
    private float expiryTime = 1f;

    [SerializeField]
    private bool beginOnAwake = true;

    [SerializeField]
    private Poolable poolable;

    private Coroutine coroutine = null;
    private WaitForSeconds expiryWait = null;

    private void OnEnable()
    {
        if (beginOnAwake)
        {
            Begin();
        }
    }

    private void OnValidate()
    {
        this.Autofill(ref poolable, true);
    }

    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void Begin()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(Expire());
    }

    IEnumerator Expire()
    {
        if (expiryWait == null)
        {
            expiryWait = new WaitForSeconds(expiryTime);
        }

        yield return expiryWait;
        if (poolable == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            poolable.Free();
        }

        coroutine = null;
    }
}

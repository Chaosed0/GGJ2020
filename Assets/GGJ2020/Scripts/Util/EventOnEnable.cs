using UnityEngine;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{
    public UnityEvent OnEnabled = new UnityEvent();
    public UnityEvent OnDisabled = new UnityEvent();

    private void OnEnable()
    {
        OnEnabled.Invoke();
    }

    private void OnDisable()
    {
        OnDisabled.Invoke();
    }
}

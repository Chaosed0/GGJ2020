using UnityEngine;
using UnityEngine.Events;

public class TriggerEventer : MonoBehaviour
{
    [System.Serializable]
    public class TriggerEnteredEvent : UnityEvent<Collider2D> { }
    public TriggerEnteredEvent OnTriggerEntered = new TriggerEnteredEvent();

    [System.Serializable]
    public class TriggerExitedEvent : UnityEvent<Collider2D> { }
    public TriggerExitedEvent OnTriggerExited = new TriggerExitedEvent();

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEntered.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExited.Invoke(other);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class EventHub
{
    public static EventHub _Instance = null;
    public static EventHub Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new EventHub();
            }

            return _Instance;
        }
    }

    public interface Event { }

    private struct Subscriber<T>
    {
        public Object owner;
        public System.Action<T> action;
    }

    private Dictionary<System.Type, HashSet<object>> subscriberEventTypeMap = new Dictionary<System.Type, HashSet<object>>();

    public void Subscribe<T>(System.Action<T> action, Object owner) where T : Event
    {
        HashSet<object> subscribers;
        if (!subscriberEventTypeMap.TryGetValue(typeof(T), out subscribers))
        {
            subscribers = new HashSet<object>();
            subscriberEventTypeMap[typeof(T)] = subscribers;
        }

        subscribers.Add(new Subscriber<T> { owner = owner, action = action });
    }

    public void Unsubscribe<T>(System.Action<T> action)
    {
        HashSet<object> subscribers;
        if (!subscriberEventTypeMap.TryGetValue(typeof(T), out subscribers))
        {
            Debug.Assert(false, $"Tried to unsubscribe from event {typeof(T)}, but event was never subscribed to");
            return;
        }

        object toRemove = null;
        foreach (Subscriber<T> subscriber in subscribers)
        {
            if (subscriber.action == action)
            {
                toRemove = subscriber;
                break;
            }
        }

        if (toRemove != null)
        {
            subscribers.Remove(toRemove);
        }
    }

    private List<object> cleanupList = new List<object>();
    public void Publish<T>(T eventParams) where T : Event
    {
        HashSet<object> subscribers;
        if (!subscriberEventTypeMap.TryGetValue(typeof(T), out subscribers))
        {
            return;
        }

        cleanupList.Clear();
        foreach (Subscriber<T> subscriber in subscribers)
        {
            if (!object.ReferenceEquals(subscriber.owner, null) && subscriber.owner == null)
            {
                cleanupList.Add(subscriber);
                continue;
            }

            try
            {
                subscriber.action(eventParams);
            }
            catch (System.Exception e)
            {
                Logger.LogError($"Exception occured while handling event: {e}", subscriber.owner);
            }
        }

        foreach (object o in cleanupList)
        {
            subscribers.Remove(o);
        }
    }
}

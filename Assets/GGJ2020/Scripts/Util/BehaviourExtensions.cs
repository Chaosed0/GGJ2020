using UnityEngine;

public static class BehaviourExtensions
{
    public static T GetComponentInterface<T>(this Behaviour behaviour) where T : class
    {
        return behaviour.GetComponent(typeof(T)) as T;
    }
}

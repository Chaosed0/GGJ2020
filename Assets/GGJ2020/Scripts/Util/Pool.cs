using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField]
    public Poolable prefab = null;

    [SerializeField]
    private int prewarmCount = 0;

    private Queue<Poolable> freeObjects = new Queue<Poolable>();
    private HashSet<Poolable> usedObjects = new HashSet<Poolable>();

    public void Prewarm()
    {
        while (freeObjects.Count + usedObjects.Count < prewarmCount)
        {
            var poolable = Spawn();
            poolable.gameObject.SetActive(false);
            poolable.transform.parent = this.transform;
            freeObjects.Enqueue(poolable);
        }
    }

    public Poolable GetObject(System.Action<Poolable> initialize = null, Transform parent = null, bool instantiateInWorldSpace = true)
    {
        Poolable poolable = null;
        if (freeObjects.Count <= 0)
        {
            poolable = Spawn(parent, instantiateInWorldSpace);
        }
        else
        {
            while (freeObjects.Count > 0 && poolable == null)
            {
                poolable = freeObjects.Dequeue();
            }

            if (freeObjects.Count <= 0 && poolable == null)
            {
                return GetObject(initialize);
            }
        }

#if UNITY_EDITOR
        Debug.Assert(!freeObjects.Contains(poolable));
#endif

        usedObjects.Add(poolable);

        foreach (IResettable resettable in poolable.gameObject.GetComponentsInChildren(typeof(IResettable)))
        {
            resettable.OnReset();
        }

        initialize?.Invoke(poolable);
        poolable.transform.parent = parent;
        poolable.gameObject.SetActive(true);

        return poolable;
    }

    private Poolable Spawn(Transform parent = null, bool instantiateInWorldSpace = true)
    {
        var poolable = Instantiate(prefab, parent, instantiateInWorldSpace);
        poolable.pool = this;
        return poolable;
    }

    public void FreeObject(Poolable poolable)
    {
        if (poolable == null)
        {
            Debug.LogError($"Tried to free a null object", this);
            return;
        }

        if (Application.isEditor && freeObjects.Contains(poolable))
        {
            Debug.LogError($"Tried to free object {poolable} that was already freed", poolable);
            return;
        }

        if (!usedObjects.Contains(poolable))
        {
            Debug.LogError($"Tried to unpool {poolable} that isn't from this pool ({this})", poolable);
            return;
        }

        usedObjects.Remove(poolable);
        poolable.gameObject.SetActive(false);
        poolable.transform.parent = this.transform;
        freeObjects.Enqueue(poolable);
    }

    private static HashSet<Poolable> usedObjectsCache = new HashSet<Poolable>();
    public void FreeAllObjects()
    {
        usedObjectsCache.Clear();
        foreach (var o in usedObjects)
        {
            usedObjectsCache.Add(o);
        }

        foreach (var o in usedObjectsCache)
        {
            FreeObject(o);
        }
    }
}

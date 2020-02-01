using UnityEngine;

public class Poolable : MonoBehaviour
{
    public Pool pool = null;

    public void Free()
    {
        if (pool != null)
        {
            pool.FreeObject(this);
        }
        else
        {
            this.LogWarning($"Pool is nonexistent, destroying instead ({this.name}) - this usually indicates that the Poolable was instantiated rather than grabbed from a pool");
            Destroy(this.gameObject);
        }
    }
}

public interface IResettable
{
    void OnReset();
}

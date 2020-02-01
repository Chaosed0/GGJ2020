using System.Collections.Generic;

public static class IEnumerableExtension
{
    /// <summary>
    /// Wraps this object instance into an IEnumerable&lt;T&gt;
    /// consisting of a single item.
    /// </summary>
    /// <typeparam name="T"> Type of the object. </typeparam>
    /// <param name="item"> The instance that will be wrapped. </param>
    /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
    public static IEnumerable<T> Yield<T>(this T item)
    {
        yield return item;
    }

    public static int GetIndexBasedOnWeights<T>(this IEnumerable<T> enumerable, float random, System.Func<T, float> GetWeight)
    {
        int i = 0;
        foreach (T o in enumerable)
        {
            if (GetWeight(o) < 0)
            {
                return i;
            }
            ++i;
        }

        i = 0;
        float currentWeight = 0f;
        foreach (T o in enumerable)
        {
            currentWeight += System.Math.Max(0f, GetWeight(o));
            if (random <= currentWeight)
            {
                return i;
            }

            ++i;
        }

        return -1;
    }

    public static T GetObjectBasedOnWeights<T>(this IEnumerable<T> enumerable, float random, System.Func<T, float> GetWeight)
    {
        foreach (T o in enumerable)
        {
            if (GetWeight(o) < 0)
            {
                return o;
            }
        }

        float currentWeight = 0f;
        foreach (T o in enumerable)
        {
            currentWeight += System.Math.Max(0f, GetWeight(o));
            if (random <= currentWeight)
            {
                return o;
            }
        }

        return default;
    }
}

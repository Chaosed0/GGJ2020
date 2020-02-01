using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeightedSampler<T>
{
    private HashSet<T> set;
    private System.Random random;
    private System.Func<T, float> GetWeight;

    public WeightedSampler(IEnumerable<T> items, System.Random random, System.Func<T, float> GetWeight)
    {
        this.set = new HashSet<T>(items);
        this.random = random;
        this.GetWeight = GetWeight;
    }

    public IEnumerable<T> ItemsWithoutReplacement()
    {
        while (set.Count > 0)
        {
            float sum = set.Sum(GetWeight);
            float sample = (float)random.NextDouble() * sum;
            T obj = set.GetObjectBasedOnWeights(sample, GetWeight);
            set.Remove(obj);
            yield return obj;
        }
    }
}

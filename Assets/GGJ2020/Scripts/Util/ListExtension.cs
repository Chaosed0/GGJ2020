using System.Collections.Generic;
using System;

public static class ListExtension
{
    public static void Resize<T>(this List<T> list, int sz, System.Func<T> construct = null)
    {
        if (sz < list.Count)
        {
            list.RemoveRange(sz, list.Count - sz);
        }
        else if (sz > list.Count)
        {
            if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
            {
                list.Capacity = sz;
            }

            while (sz > list.Count)
            {
                if (construct != null)
                {
                    list.Add(construct());
                }
                else
                {
                    list.Add(default(T));
                }
            }
        }
    }

    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list, Random rng = null)
    {
        if (rng == null)
        {
            rng = ListExtension.rng;
        }

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}


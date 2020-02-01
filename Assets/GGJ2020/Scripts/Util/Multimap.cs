using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MultiMap<K, V>: IEnumerable<KeyValuePair<K, V>>
{
    Dictionary<K, List<V>> _dictionary =
        new Dictionary<K, List<V>>();

    public void Add(K key, V value)
    {
        // Add a key.
        List<V> list;
        if (this._dictionary.TryGetValue(key, out list))
        {
            list.Add(value);
        }
        else
        {
            list = new List<V>();
            list.Add(value);
            this._dictionary[key] = list;
        }
    }

    public void Clear()
    {
        foreach (var pair in _dictionary)
        {
            pair.Value.Clear();
        }
    }

    public IEnumerable<K> Keys
    {
        get
        {
            // Get all keys.
            return this._dictionary.Keys;
        }
    }

    public List<V> this[K key]
    {
        get
        {
            // Get list at a key.
            List<V> list;
            if (!this._dictionary.TryGetValue(key, out list))
            {
                list = new List<V>();
                this._dictionary[key] = list;
            }
            return list;
        }
    }

    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    {
        foreach (var pair in _dictionary)
        {
            foreach (var val in pair.Value)
            {
                yield return new KeyValuePair<K, V>(pair.Key, val);
            }
        }
    }

    public int Count
    {
        get
        {
            return _dictionary.Aggregate(0, (acc, pair) => acc + pair.Value.Count);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

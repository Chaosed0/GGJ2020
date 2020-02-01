using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class EnumMap<EnumType, ValueType>
    where EnumType : Enum
{
    [SerializeField]
    private List<ValueType> values = new List<ValueType>();

    public EnumMap()
    {
        for (int i = 0; i < Enum.GetValues(typeof(EnumType)).Length; ++i)
        {
            values.Add(default);
        }
    }

    public ValueType GetValue(EnumType enumValue)
    {
        return values[(int)(object)enumValue];
    }

    public EnumType GetEnum(ValueType value)
    {
        return (EnumType)(object)values.IndexOf(value);
    }

    public void SetValue(EnumType enumValue, ValueType weapon)
    {
        values[(int)(object)enumValue] = weapon;
    }

    public IEnumerable<KeyValuePair<EnumType, ValueType>> GetPairs()
    {
        for (int i = 0; i < values.Count; ++i)
        {
            yield return new KeyValuePair<EnumType, ValueType>((EnumType)(object)i, values[i]);
        }
    }
}

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class KeyValueFormatParser
{
    private const string DataPath = "data.data";
    public static Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

    [RuntimeInitializeOnLoadMethod]
    private static void ParseData()
    {
        // Debug.Log("KeyValueFormatParser::ParseData()");
        var path = MetaLoadUtil.GetPath(DataPath);
        keyValuePairs = KeyValueFormatParser.Parse(path);
        if (keyValuePairs == null)
        {
            Debug.LogError($"Data file does not exist at {path}!");
        }
    }

    public static Dictionary<string, string> Parse(string path)
    {
        var keyValuePairs = new Dictionary<string, string>();

        if (!File.Exists(path))
        {
            return null;
        }

        using (var reader = new StreamReader(path))
        {
            while (reader.Peek() != -1)
            {
                ParseLine(reader.ReadLine(), keyValuePairs);
            }
        }

        return keyValuePairs;
    }

    private static void ParseLine(string line, Dictionary<string, string> keyValuePairs)
    {
        int equalsIndex = line.IndexOf('=');
        if (equalsIndex < 0)
        {
            return;
        }

        string key = line.Substring(0, equalsIndex).Trim();
        string value = line.Substring(equalsIndex + 1).Trim();
        keyValuePairs[key] = value;
    }
}

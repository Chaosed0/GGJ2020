using System.IO;
using System.Collections.Generic;

public class KeyValueFormatParser
{
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
        string key = line.Substring(0, equalsIndex).Trim();
        string value = line.Substring(equalsIndex + 1).Trim();
        keyValuePairs[key] = value;
    }
}

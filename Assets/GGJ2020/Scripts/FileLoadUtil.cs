using UnityEngine;
using System.Collections;
using System.IO;

public class MetaLoadUtil
{
    public const string EditorTestPath = "TestAssets/TestingDirectory/";

    public static string GetPath(string path)
    {
        if (Application.isEditor)
        {
            path = Path.Combine(MetaLoadUtil.EditorTestPath, path);
        }

        return path;
    }
}

﻿using UnityEngine;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

public class CopyCleanAssetsPostBuild
{
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        var sourceDirectory = "TestAssets/Clean";
        var destDirectory = new FileInfo(pathToBuiltProject).Directory.FullName;

        foreach (var sourceSubDirectory in new DirectoryInfo(sourceDirectory).GetDirectories())
        {
            var destSubDirectory = Path.Combine(destDirectory, sourceSubDirectory.Name);

            if (Directory.Exists(destSubDirectory))
            {
                Directory.Delete(destSubDirectory, true);
            }

            CopyDir.Copy(sourceSubDirectory.FullName, destSubDirectory);
        }
    }
}

class CopyDir
{
    public static void Copy(string sourceDirectory, string targetDirectory)
    {
        DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
        DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

        CopyAll(diSource, diTarget);
    }

    public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        Directory.CreateDirectory(target.FullName);

        // Copy each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
            fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }
}
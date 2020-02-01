using UnityEngine;

public class Logger
{
    public enum Level
    {
        Error,
        Warning,
        Info,
        Verbose,
        Verbose2,
        Verbose3,
    }

    public static void LogError(string message, UnityEngine.Object context = null)
    {
        Logger.Log(Level.Error, message, context);
    }

    public static void LogWarning(string message, UnityEngine.Object context = null)
    {
        Logger.Log(Level.Warning, message, context);
    }

    public static void LogInfo(string message, UnityEngine.Object context = null)
    {
        Logger.Log(Level.Info, message, context);
    }

    public static void LogVerbose(string message, UnityEngine.Object context = null)
    {
        Logger.Log(Level.Verbose, message, context);
    }

    public static void LogVerbose2(string message, UnityEngine.Object context = null)
    {
        Logger.Log(Level.Verbose2, message, context);
    }

    public static void LogVerbose3(string message, UnityEngine.Object context = null)
    {
        Logger.Log(Level.Verbose3, message, context);
    }

    public static void Log(Level level, string message, UnityEngine.Object context = null)
    {
        var setting = Logger.GetLogLevel(context);
        if (level > setting.level)
        {
            return;
        }

        if (level > setting.stackTraceLogLevel)
        {
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
        }
        else
        {
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
        }

        if (level == Level.Error)
        {
            Debug.LogError(message, context);
        }
        else if (level == Level.Warning)
        {
            Debug.LogWarning(message, context);
        }
        else if (level == Level.Info)
        {
            Debug.Log(message, context);
        }
        else
        {
            Debug.Log($"{context?.name}: {message}", context);
        }
    }

    private static LogSettings.Override GetLogLevel(UnityEngine.Object context)
    {
        if (LogSettings.Instance != null)
        {
            return LogSettings.Instance.GetLogSettingForObject(context);
        }

        return new LogSettings.Override { level = Level.Info, stackTraceLogLevel = Level.Info };
    }
}

public static class LoggerExtensions
{
    public static void LogError(this UnityEngine.Object o, string message)
    {
        Logger.LogError(message, o);
    }

    public static void LogWarning(this UnityEngine.Object o, string message)
    {
        Logger.LogWarning(message, o);
    }

    public static void LogInfo(this UnityEngine.Object o, string message)
    {
        Logger.LogInfo(message, o);
    }

    public static void LogVerbose(this UnityEngine.Object o, string message)
    {
        Logger.LogVerbose(message, o);
    }

    public static void LogVerbose2(this UnityEngine.Object o, string message)
    {
        Logger.LogVerbose2(message, o);
    }

    public static void LogVerbose3(this UnityEngine.Object o, string message)
    {
        Logger.LogVerbose3(message, o);
    }
}

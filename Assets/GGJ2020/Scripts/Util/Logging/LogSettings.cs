using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(menuName = "Obelus/LogSettings")]
public class LogSettings : ScriptableObject
{
    private static Override globalOverride = new Override { pattern = "global" };

    [System.Serializable]
    public class Override
    {
        [Sirenix.OdinInspector.ToggleGroup("enabled", "$pattern")]
        public bool enabled = true;

        [Sirenix.OdinInspector.ToggleGroup("enabled")]
        public string pattern = "";

        [Sirenix.OdinInspector.ToggleGroup("enabled")]
        public Logger.Level level = Logger.Level.Info;

        [Sirenix.OdinInspector.ToggleGroup("enabled")]
        public Logger.Level stackTraceLogLevel = Logger.Level.Info;
    }

    [SerializeField]
    private Logger.Level globalLevel = Logger.Level.Info;

    [SerializeField]
    private Logger.Level stackTraceLogLevel = Logger.Level.Info;

    [SerializeField]
    [Sirenix.OdinInspector.ListDrawerSettings(ShowPaging = false, ShowItemCount = false)]
    private List<Override> overrides = new List<Override>();

    public Override GetLogSettingForObject(Object o)
    {
        if (o != null)
        {
            foreach (Override levelOverride in overrides)
            {
                if (levelOverride.enabled && Regex.Match(o.ToString(), levelOverride.pattern).Success)
                {
                    return levelOverride;
                }
            }
        }

        globalOverride.level = globalLevel;
        globalOverride.stackTraceLogLevel = this.stackTraceLogLevel;
        return globalOverride;
    }

    static LogSettings _instance = null;
    public static LogSettings Instance
    {
        get
        {
            if (!_instance)
            {
                LogSettings[] mappers = Resources.FindObjectsOfTypeAll<LogSettings>();
                if (mappers.Length <= 0)
                {
                    return null;
                }

                _instance = mappers[0];
            }

            return _instance;
        }
    }
}

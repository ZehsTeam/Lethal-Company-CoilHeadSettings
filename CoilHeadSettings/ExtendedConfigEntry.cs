using BepInEx.Configuration;
using System;

namespace com.github.zehsteam.CoilHeadSettings;

public class ExtendedConfigEntry<T>
{
    public ConfigEntry<T> ConfigEntry;
    public Func<T> GetValue;
    public Action<T> SetValue;

    public T DefaultValue => (T)ConfigEntry.DefaultValue;
    public T Value { get { return GetValue(); } set { SetValue(value); } }

    public ExtendedConfigEntry(string section, string key, T defaultValue, string description)
    {
        ConfigEntry = Plugin.Instance.Config.Bind(section, key, defaultValue, description);
        Initialize();
    }

    public ExtendedConfigEntry(string section, string key, T defaultValue, ConfigDescription configDescription = null)
    {
        ConfigEntry = Plugin.Instance.Config.Bind(section, key, defaultValue, configDescription);
        Initialize();
    }

    private void Initialize()
    {
        if (GetValue == null)
        {
            GetValue = () =>
            {
                return ConfigEntry.Value;
            };
        }

        if (SetValue == null)
        {
            SetValue = (T value) =>
            {
                ConfigEntry.Value = value;
            };
        }
    }

    public void ResetToDefault()
    {
        ConfigEntry.Value = (T)ConfigEntry.DefaultValue;
    }
}

using BepInEx.Configuration;
using com.github.zehsteam.CoilHeadSettings.Patches;

namespace com.github.zehsteam.CoilHeadSettings;

internal class ConfigManager
{
    // General
    public ConfigEntry<bool> General_ExtendedLogging { get; private set; }

    // Enemy
    public ConfigEntry<float> Enemy_PowerLevel { get; private set; }
    public ConfigEntry<float> Enemy_MovementSpeed { get; private set; }
    public ConfigEntry<int> Enemy_AttackDamage { get; private set; }
    public ConfigEntry<float> Enemy_AttackSpeed { get; private set; }
    public ConfigEntry<string> Enemy_ProbabilityCurve { get; private set; }

    public ConfigManager()
    {
        BindConfigs();
        SetupChangedEvents();
        MigrateOldConfigSettings();
    }

    private void BindConfigs()
    {
        ConfigHelper.SkipAutoGen();

        // General
        General_ExtendedLogging = ConfigHelper.Bind("General", "ExtendedLogging", defaultValue: false, requiresRestart: false, "Enable extended logging.");

        // Enemy
        Enemy_PowerLevel =       ConfigHelper.Bind("Enemy", "PowerLevel",       defaultValue: 1f,              requiresRestart: false, $"The power level of {EnemyDataManager.EnemyDisplayName}.");
        Enemy_MovementSpeed =    ConfigHelper.Bind("Enemy", "MovementSpeed",    defaultValue: 14.5f,           requiresRestart: false, $"The movement speed of {EnemyDataManager.EnemyDisplayName}.");
        Enemy_AttackDamage =     ConfigHelper.Bind("Enemy", "AttackDamage",     defaultValue: 90,              requiresRestart: false, $"The attack damage of {EnemyDataManager.EnemyDisplayName}.");
        Enemy_AttackSpeed =      ConfigHelper.Bind("Enemy", "AttackSpeed",      defaultValue: 5f,              requiresRestart: false, $"The number of times {EnemyDataManager.EnemyDisplayName} will attack per second.");
        Enemy_ProbabilityCurve = ConfigHelper.Bind("Enemy", "ProbabilityCurve", defaultValue: "1.0, 1.0, 1.0", requiresRestart: false, $"Determines how likely {EnemyDataManager.EnemyDisplayName} is to spawn throughout the day. Accepts an array of floats with each entry separated by a comma.");
    }

    private void SetupChangedEvents()
    {
        // Enemy
        Enemy_PowerLevel.SettingChanged += Enemy_PowerLevel_SettingChanged;
        Enemy_MovementSpeed.SettingChanged += (object sender, System.EventArgs e) => SpringManAIPatch.SettingsChanged();
        Enemy_ProbabilityCurve.SettingChanged += Enemy_ProbabilityCurve_SettingChanged;
    }

    private void Enemy_PowerLevel_SettingChanged(object sender, System.EventArgs e)
    {
        EnemyHelper.SetPowerLevel(EnemyDataManager.EnemyName, Enemy_PowerLevel.Value);
    }

    private void Enemy_ProbabilityCurve_SettingChanged(object sender, System.EventArgs e)
    {
        EnemyHelper.SetProbabilityCurve(EnemyDataManager.EnemyName, Utils.ToFloatsArray(Enemy_ProbabilityCurve.Value));
    }

    private void MigrateOldConfigSettings()
    {
        foreach (var entry in ConfigHelper.GetOrphanedConfigEntries())
        {
            MigrateOldConfigSetting(entry.Key.Section, entry.Key.Key, entry.Value);
        }
    }

    private void MigrateOldConfigSetting(string section, string key, string value)
    {
        if (section == "General Settings")
        {
            switch (key)
            {
                case "ExtendedLogging":
                    ConfigHelper.SetConfigEntryValue(General_ExtendedLogging, value);
                    break;
            }
        }

        if (section == "Coil-Head Settings")
        {
            switch (key)
            {
                case "PowerLevel":
                    ConfigHelper.SetConfigEntryValue(Enemy_PowerLevel, value);
                    break;
                case "MovementSpeed":
                    ConfigHelper.SetConfigEntryValue(Enemy_MovementSpeed, value);
                    break;
                case "AttackDamage":
                    ConfigHelper.SetConfigEntryValue(Enemy_AttackDamage, value);
                    break;
                case "AttackSpeed":
                    ConfigHelper.SetConfigEntryValue(Enemy_AttackSpeed, value);
                    break;
            }
        }
    }
}

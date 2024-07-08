using BepInEx.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace com.github.zehsteam.CoilHeadSettings;

internal class SyncedConfigManager
{
    public SyncedConfigData _hostConfigData;

    // General Settings
    public ExtendedConfigEntry<bool> ExtendedLogging { get; private set; }

    // Coil-Head Settings
    public ExtendedConfigEntry<float> PowerLevel { get; private set; }
    public ExtendedConfigEntry<float> MovementSpeed { get; private set; }
    public ExtendedConfigEntry<int> AttackDamage { get; private set; }
    public ExtendedConfigEntry<float> AttackSpeed { get; private set; }
    public ExtendedConfigEntry<string> SpawnSettingsMoonList { get; private set; }

    public SyncedConfigManager()
    {
        BindConfigs();
        ClearUnusedEntries();
    }

    private void BindConfigs()
    {
        ConfigFile configFile = Plugin.Instance.Config;

        // General Settings
        ExtendedLogging = new("General Settings", "ExtendedLogging", defaultValue: false, "Enable extended logging.");

        // Coil-Head Settings
        PowerLevel = new("Coil-Head Settings", "PowerLevel", defaultValue: 1f, "The power level of the Coil-Head.");

        MovementSpeed = new("Coil-Head Settings", "MovementSpeed", defaultValue: 14.5f, "The movement speed of the Coil-Head.");
        MovementSpeed.GetValue = () =>
        {
            return _hostConfigData == null ? MovementSpeed.ConfigEntry.Value : _hostConfigData.MovementSpeed;
        };
        MovementSpeed.ConfigEntry.SettingChanged += SyncedConfigSettingsChanged;

        AttackDamage = new("Coil-Head Settings", "AttackDamage", defaultValue: 90, "How much damage the Coil-Head will deal.");
        AttackDamage.GetValue = () =>
        {
            return _hostConfigData == null ? AttackDamage.ConfigEntry.Value : _hostConfigData.AttackDamage;
        };
        AttackDamage.ConfigEntry.SettingChanged += SyncedConfigSettingsChanged;

        AttackSpeed = new("Coil-Head Settings", "AttackSpeed", defaultValue: 5f, "How many times the Coil-Head will attack per second.");
        AttackSpeed.GetValue = () =>
        {
            return _hostConfigData == null ? AttackSpeed.ConfigEntry.Value : _hostConfigData.AttackSpeed;
        };
        AttackSpeed.ConfigEntry.SettingChanged += SyncedConfigSettingsChanged;

        string spawnSettingsMoonListDescription = "Coil-Head spawn settings list for moons.\n";
        spawnSettingsMoonListDescription += "Separate each entry with a comma.\n";
        spawnSettingsMoonListDescription += "PlanetName:MaxSpawnCount:Rarity\n";
        spawnSettingsMoonListDescription += "<string>:<int>:<int>\n";
        SpawnSettingsMoonList = new("Coil-Head Settings", "SpawnSettingsMoonList", defaultValue: "41 Experimentation:5:0, 220 Assurance:5:0, 56 Vow:5:6, 21 Offense:5:25, 61 March:5:10, 20 Adamance:5:10, 85 Rend:5:43, 7 Dine:5:6, 8 Titan:5:59, 68 Artifice:5:88, 5 Embrion:5:25", spawnSettingsMoonListDescription);
    }

    private void ClearUnusedEntries()
    {
        ConfigFile configFile = Plugin.Instance.Config;

        // Normally, old unused config entries don't get removed, so we do it with this piece of code. Credit to Kittenji.
        PropertyInfo orphanedEntriesProp = configFile.GetType().GetProperty("OrphanedEntries", BindingFlags.NonPublic | BindingFlags.Instance);
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(configFile, null);
        orphanedEntries.Clear(); // Clear orphaned entries (Unbinded/Abandoned entries)
        configFile.Save(); // Save the config file to save these changes
    }

    public void SetHostConfigData(SyncedConfigData syncedConfigData)
    {
        _hostConfigData = syncedConfigData;
    }

    private void SyncedConfigSettingsChanged(object sender, System.EventArgs e)
    {
        SyncedConfigSettingsChanged();
    }

    private void SyncedConfigSettingsChanged()
    {
        if (!Plugin.IsHostOrServer) return;

        PluginNetworkBehaviour.Instance.SendConfigToPlayerClientRpc(new SyncedConfigData(this));
    }
}

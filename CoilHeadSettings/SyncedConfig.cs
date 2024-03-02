using BepInEx.Configuration;

namespace com.github.zehsteam.CoilHeadSettings;

internal class SyncedConfig
{
    private SyncedConfigData hostConfigData;

    private ConfigEntry<int> PowerLevelCfg;
    private ConfigEntry<int> MaxSpawnedCfg;
    private ConfigEntry<int> AttackDamageCfg;
    private ConfigEntry<float> AttackSpeedCfg;
    private ConfigEntry<float> MovementSpeedCfg;

    internal int PowerLevel
    {
        get
        {
            return hostConfigData == null ? PowerLevelCfg.Value : hostConfigData.powerLevel;
        }
        set
        {
            PowerLevelCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int MaxSpawned
    {
        get
        {
            return hostConfigData == null ? MaxSpawnedCfg.Value : hostConfigData.maxSpawned;
        }
        set
        {
            MaxSpawnedCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int AttackDamage
    {
        get
        {
            return hostConfigData == null ? AttackDamageCfg.Value : hostConfigData.attackDamage;
        }
        set
        {
            AttackDamageCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal float AttackSpeed
    {
        get
        {
            return hostConfigData == null ? AttackSpeedCfg.Value : hostConfigData.attackSpeed;
        }
        set
        {
            AttackSpeedCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal float MovementSpeed
    {
        get
        {
            return hostConfigData == null ? MovementSpeedCfg.Value : hostConfigData.movementSpeed;
        }
        set
        {
            MovementSpeedCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    public SyncedConfig()
    {
        BindConfigs();
    }

    private void BindConfigs()
    {
        ConfigFile config = CoilHeadSettingsBase.Instance.Config;

        PowerLevelCfg = config.Bind(
            new ConfigDefinition("Settings", "powerLevel"),
            1,
            new ConfigDescription("The power level of the Coil-Head.")
        );
        MaxSpawnedCfg = config.Bind(
            new ConfigDefinition("Settings", "maxSpawned"),
            5,
            new ConfigDescription("The max amount of Coil-Heads that can spawn.")
        );
        AttackDamageCfg = config.Bind(
            new ConfigDefinition("Settings", "attackDamage"),
            90,
            new ConfigDescription("How much damage the Coil-Head will deal.")
        );
        AttackSpeedCfg = config.Bind(
            new ConfigDefinition("Settings", "attackSpeed"),
            5f,
            new ConfigDescription("How many times the Coil-Head will attack per second.")
        );
        MovementSpeedCfg = config.Bind(
            new ConfigDefinition("Settings", "movementSpeed"),
            14.5f,
            new ConfigDescription("The movement speed of the Coil-Head.")
        );
    }

    public void SetHostConfigData(SyncedConfigData syncedConfigData)
    {
        hostConfigData = syncedConfigData;
    }

    private void SyncedConfigsChanged()
    {
        if (!CoilHeadSettingsBase.IsHostOrServer) return;

        PluginNetworkBehaviour.Instance.SendConfigToPlayerClientRpc(new SyncedConfigData(this));
    }
}

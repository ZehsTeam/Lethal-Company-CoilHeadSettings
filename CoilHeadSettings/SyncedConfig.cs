using BepInEx.Configuration;

namespace com.github.zehsteam.CoilHeadSettings;

internal class SyncedConfig
{
    private SyncedConfigData hostConfigData;

    // Settings
    private ConfigEntry<float> PowerLevelCfg;
    private ConfigEntry<int> AttackDamageCfg;
    private ConfigEntry<float> AttackSpeedCfg;
    private ConfigEntry<float> MovementSpeedCfg;

    // Spawn Settings
    private ConfigEntry<int> MaxSpawnCountCfg;
    private ConfigEntry<float> SpawnWeightMultiplierCfg;
    private ConfigEntry<int> LiquidationSpawnWeightCfg;
    private ConfigEntry<int> EmbrionSpawnWeightCfg;
    private ConfigEntry<int> ArtificeSpawnWeightCfg;
    private ConfigEntry<int> TitanSpawnWeightCfg;
    private ConfigEntry<int> DineSpawnWeightCfg;
    private ConfigEntry<int> RendSpawnWeightCfg;
    private ConfigEntry<int> AdamanceSpawnWeightCfg;
    private ConfigEntry<int> MarchSpawnWeightCfg;
    private ConfigEntry<int> OffenseSpawnWeightCfg;
    private ConfigEntry<int> VowSpawnWeightCfg;

    // Settings
    internal float PowerLevel
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

    // Spawn Settings
    internal int MaxSpawnCount
    {
        get
        {
            return hostConfigData == null ? MaxSpawnCountCfg.Value : hostConfigData.maxSpawnCount;
        }
        set
        {
            MaxSpawnCountCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal float SpawnWeightMultiplier
    {
        get
        {
            return hostConfigData == null ? SpawnWeightMultiplierCfg.Value : hostConfigData.spawnWeightMultiplier;
        }
        set
        {
            SpawnWeightMultiplierCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int LiquidationSpawnWeight
    {
        get
        {
            return hostConfigData == null ? LiquidationSpawnWeightCfg.Value : hostConfigData.liquidationSpawnWeight;
        }
        set
        {
            LiquidationSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int EmbrionSpawnWeight
    {
        get
        {
            return hostConfigData == null ? EmbrionSpawnWeightCfg.Value : hostConfigData.embrionSpawnWeight;
        }
        set
        {
            EmbrionSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int ArtificeSpawnWeight
    {
        get
        {
            return hostConfigData == null ? ArtificeSpawnWeightCfg.Value : hostConfigData.artificeSpawnWeight;
        }
        set
        {
            ArtificeSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int TitanSpawnWeight
    {
        get
        {
            return hostConfigData == null ? TitanSpawnWeightCfg.Value : hostConfigData.titanSpawnWeight;
        }
        set
        {
            TitanSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int DineSpawnWeight
    {
        get
        {
            return hostConfigData == null ? DineSpawnWeightCfg.Value : hostConfigData.dineSpawnWeight;
        }
        set
        {
            DineSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int RendSpawnWeight
    {
        get
        {
            return hostConfigData == null ? RendSpawnWeightCfg.Value : hostConfigData.rendSpawnWeight;
        }
        set
        {
            RendSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int AdamanceSpawnWeight
    {
        get
        {
            return hostConfigData == null ? AdamanceSpawnWeightCfg.Value : hostConfigData.adamanceSpawnWeight;
        }
        set
        {
            AdamanceSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int OffenseSpawnWeight
    {
        get
        {
            return hostConfigData == null ? OffenseSpawnWeightCfg.Value : hostConfigData.offenseSpawnWeight;
        }
        set
        {
            OffenseSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int MarchSpawnWeight
    {
        get
        {
            return hostConfigData == null ? MarchSpawnWeightCfg.Value : hostConfigData.marchSpawnWeight;
        }
        set
        {
            MarchSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    internal int VowSpawnWeight
    {
        get
        {
            return hostConfigData == null ? VowSpawnWeightCfg.Value : hostConfigData.vowSpawnWeight;
        }
        set
        {
            VowSpawnWeightCfg.Value = value;
            SyncedConfigsChanged();
        }
    }

    public SyncedConfig()
    {
        BindConfigs();
    }

    private void BindConfigs()
    {
        ConfigFile config = Plugin.Instance.Config;

        // Settings
        PowerLevelCfg = config.Bind(
            new ConfigDefinition("Settings", "powerLevel"),
            1f,
            new ConfigDescription("The power level of the Coil-Head.")
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

        // Spawn Settings
        MaxSpawnCountCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "maxSpawnCount"),
            5,
            new ConfigDescription("The max amount of Coil-Heads that can spawn.")
        );
        SpawnWeightMultiplierCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "spawnWeightMultiplier"),
            1f,
            new ConfigDescription("The global spawn chance weight multiplier for Coil-Heads.")
        );
        LiquidationSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "liquidationSpawnWeight"),
            44,
            new ConfigDescription("The Coil-Head spawn chance weight for 44-Liquidation.",
            new AcceptableValueRange<int>(0, 100))
        );
        EmbrionSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "embrionSpawnWeight"),
            25,
            new ConfigDescription("The Coil-Head spawn chance weight for 5-Embrion.",
            new AcceptableValueRange<int>(0, 100))
        );
        ArtificeSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "artificeSpawnWeight"),
            88,
            new ConfigDescription("The Coil-Head spawn chance weight for 68-Artifice.",
            new AcceptableValueRange<int>(0, 100))
        );
        TitanSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "titanSpawnWeight"),
            59,
            new ConfigDescription("The Coil-Head spawn chance weight for 8-Titan.",
            new AcceptableValueRange<int>(0, 100))
        );
        DineSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "dineSpawnWeight"),
            6,
            new ConfigDescription("The Coil-Head spawn chance weight for 7-Dine.",
            new AcceptableValueRange<int>(0, 100))
        );
        RendSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "rendSpawnWeight"),
            43,
            new ConfigDescription("The Coil-Head spawn chance weight for 85-Rend.",
            new AcceptableValueRange<int>(0, 100))
        );
        AdamanceSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "adamanceSpawnWeight"),
            10,
            new ConfigDescription("The Coil-Head spawn chance weight for 20-Adamance.",
            new AcceptableValueRange<int>(0, 100))
        );
        OffenseSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "offenseSpawnWeight"),
            25,
            new ConfigDescription("The Coil-Head spawn chance weight for 21-Offense.",
            new AcceptableValueRange<int>(0, 100))
        );
        MarchSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "marchSpawnWeight"),
            10,
            new ConfigDescription("The Coil-Head spawn chance weight for 61-March.",
            new AcceptableValueRange<int>(0, 100))
        );
        VowSpawnWeightCfg = config.Bind(
            new ConfigDefinition("Spawn Settings", "vowSpawnWeight"),
            6,
            new ConfigDescription("The Coil-Head spawn chance weight for 56-Vow.",
            new AcceptableValueRange<int>(0, 100))
        );
    }

    public void SetHostConfigData(SyncedConfigData syncedConfigData)
    {
        hostConfigData = syncedConfigData;
    }

    private void SyncedConfigsChanged()
    {
        if (!Plugin.IsHostOrServer) return;

        PluginNetworkBehaviour.Instance.SendConfigToPlayerClientRpc(new SyncedConfigData(this));
    }
}

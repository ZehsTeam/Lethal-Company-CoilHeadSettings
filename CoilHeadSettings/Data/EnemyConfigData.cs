using BepInEx.Configuration;
using com.github.zehsteam.CoilHeadSettings.Dependencies;
using System;

namespace com.github.zehsteam.CoilHeadSettings.Data;

public class EnemyConfigData
{
    public EnemyConfigDataDefault DefaultValues { get; private set; }

    public ConfigEntry<int> SpawnWeight { get; private set; }
    public ConfigEntry<int> MaxSpawnCount { get; private set; }
    public ConfigEntry<bool> SpawnInside { get; private set; }
    public ConfigEntry<bool> SpawnOutside { get; private set; }

    public EnemyData EnemyData { get; private set; }

    public EnemyConfigData()
    {
        DefaultValues = new EnemyConfigDataDefault();
    }

    public EnemyConfigData(EnemyConfigDataDefault defaultValues)
    {
        DefaultValues = defaultValues;
    }

    public void BindConfigs(EnemyData enemyData)
    {
        DefaultValues ??= new EnemyConfigDataDefault();

        if (enemyData == null)
        {
            return;
        }

        EnemyData = enemyData;

        string section = EnemyData.PlanetName;
        
        SpawnWeight =   ConfigHelper.Bind(section, "SpawnWeight",   defaultValue: DefaultValues.SpawnWeight,   requiresRestart: false, $"The spawn weight of {EnemyDataManager.EnemyDisplayName}.");
        MaxSpawnCount = ConfigHelper.Bind(section, "MaxSpawnCount", defaultValue: DefaultValues.MaxSpawnCount, requiresRestart: false, $"The max amount of {EnemyDataManager.EnemyDisplayName} that can spawn.");

        SpawnWeight.SettingChanged += SpawnWeight_SettingChanged;
        MaxSpawnCount.SettingChanged += MaxSpawnCount_SettingChanged;

        if (StarlancerAIFixProxy.Enabled)
        {
            SpawnInside =  ConfigHelper.Bind(section, "SpawnInside",  defaultValue: DefaultValues.SpawnInside,  requiresRestart: false, $"If enabled, {EnemyDataManager.EnemyDisplayName} will be able to spawn inside.");
            SpawnOutside = ConfigHelper.Bind(section, "SpawnOutside", defaultValue: DefaultValues.SpawnOutside, requiresRestart: false, $"If enabled, {EnemyDataManager.EnemyDisplayName} will be able to spawn outside.");

            SpawnInside.SettingChanged += SpawnInside_SettingChanged;
            SpawnOutside.SettingChanged += SpawnOutside_SettingChanged;
        }
    }

    private void SpawnWeight_SettingChanged(object sender, EventArgs e)
    {
        if (SpawnInside == null || SpawnInside.Value)
        {
            EnemyHelper.SetSpawnWeight(EnemyDataManager.EnemyName, SpawnWeight.Value, EnemyListType.Inside, EnemyData.PlanetName);
        }

        if (SpawnOutside != null && SpawnOutside.Value)
        {
            EnemyHelper.SetSpawnWeight(EnemyDataManager.EnemyName, SpawnWeight.Value, EnemyListType.Outside, EnemyData.PlanetName);
        }
    }

    private void MaxSpawnCount_SettingChanged(object sender, EventArgs e)
    {
        EnemyHelper.SetMaxSpawnCount(EnemyDataManager.EnemyName, MaxSpawnCount.Value, EnemyData.PlanetName);
    }

    private void SpawnInside_SettingChanged(object sender, EventArgs e)
    {
        if (SpawnInside.Value)
        {
            LevelHelper.AddEnemyToLevel(EnemyData.PlanetName, EnemyDataManager.EnemyName, SpawnWeight.Value, EnemyListType.Inside);
        }
        else
        {
            LevelHelper.RemoveEnemyFromLevel(EnemyData.PlanetName, EnemyDataManager.EnemyName, EnemyListType.Inside);
        }
    }

    private void SpawnOutside_SettingChanged(object sender, EventArgs e)
    {
        if (SpawnOutside.Value)
        {
            LevelHelper.AddEnemyToLevel(EnemyData.PlanetName, EnemyDataManager.EnemyName, SpawnWeight.Value, EnemyListType.Outside);
        }
        else
        {
            LevelHelper.RemoveEnemyFromLevel(EnemyData.PlanetName, EnemyDataManager.EnemyName, EnemyListType.Outside);
        }
    }
}

public class EnemyConfigDataDefault
{
    public int SpawnWeight = 1;
    public int MaxSpawnCount = 5;
    public bool SpawnInside = true;
    public bool SpawnOutside = false;

    public EnemyConfigDataDefault()
    {

    }

    public EnemyConfigDataDefault(int spawnWeight, int maxSpawnCount, bool spawnInside, bool spawnOutside)
    {
        SpawnWeight = spawnWeight;
        MaxSpawnCount = maxSpawnCount;
        SpawnInside = spawnInside;
        SpawnOutside = spawnOutside;
    }
}

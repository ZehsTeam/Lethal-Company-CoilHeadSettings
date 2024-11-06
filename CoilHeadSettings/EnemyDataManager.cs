﻿using com.github.zehsteam.CoilHeadSettings.Data;
using System.Collections.Generic;
using UnityEngine;

namespace com.github.zehsteam.CoilHeadSettings;

internal static class EnemyDataManager
{
    public static List<EnemyData> EnemyDataList { get; private set; } = [];

    public const string EnemyName = "Spring";
    public const string EnemyDisplayName = "Coil-Head";

    private static bool _initialized;

    public static void Initialize()
    {
        if (_initialized) return;
        _initialized = true;

        InitializeEnemyDataList();
    }

    private static void InitializeEnemyDataList()
    {
        if (StartOfRound.Instance == null)
        {
            Plugin.Logger.LogError($"Failed to initialize enemy data list. StartOfRound Instance is null. (EnemyName: \"{EnemyName}\")");
            return;
        }

        EnemyType enemyType = EnemyHelper.GetEnemyType(EnemyName);

        if (enemyType == null)
        {
            Plugin.Logger.LogError($"Failed to initialize enemy data list. EnemyType is null. (EnemyName: \"{EnemyName}\")");
            return;
        }

        enemyType.PowerLevel = Plugin.ConfigManager.Enemy_PowerLevel.Value;

        EnemyHelper.SetProbabilityCurve(EnemyName, Utils.ToFloatsArray(Plugin.ConfigManager.Enemy_ProbabilityCurve.Value));

        foreach (var level in StartOfRound.Instance.levels)
        {
            if (!level.spawnEnemiesAndScrap) continue;

            AddEnemyData(level);
        }

        foreach (var enemyData in EnemyDataList)
        {
            int spawnWeight = enemyData.ConfigData.SpawnWeight.Value;

            if (enemyData.ConfigData.SpawnInside == null || enemyData.ConfigData.SpawnInside.Value)
            {
                LevelHelper.AddEnemyToLevel(enemyData.PlanetName, EnemyName, spawnWeight, EnemyListType.Inside);
            }
            else
            {
                LevelHelper.RemoveEnemyFromLevel(enemyData.PlanetName, EnemyName, EnemyListType.Inside);
            }

            if (enemyData.ConfigData.SpawnOutside != null && enemyData.ConfigData.SpawnOutside.Value)
            {
                LevelHelper.AddEnemyToLevel(enemyData.PlanetName, EnemyName, spawnWeight, EnemyListType.Outside);
            }
            else
            {
                LevelHelper.RemoveEnemyFromLevel(enemyData.PlanetName, EnemyName, EnemyListType.Outside);
            }
        }

        ConfigHelper.ClearUnusedEntries();
    }

    public static void AddEnemyData(SelectableLevel level)
    {
        if (HasEnemyData(level.PlanetName))
        {
            Plugin.Logger.LogWarning($"Failed to add enemy data for level \"{level.PlanetName}\". Enemy data already exists for SelectableLevel.");
            return;
        }

        EnemyType enemyType = EnemyHelper.GetEnemyType(EnemyName);

        if (enemyType == null)
        {
            Plugin.Logger.LogError($"Failed to add enemy data for level \"{level.PlanetName}\". EnemyType is null");
            return;
        }

        bool hasInsideEnemy = LevelHelper.LevelHasEnemy(level.PlanetName, EnemyName, EnemyListType.Inside, out int insideSpawnWeight);
        bool hasOutsideEnemy = LevelHelper.LevelHasEnemy(level.PlanetName, EnemyName, EnemyListType.Outside, out int outsideSpawnWeight);

        int spawnWeight = 0;

        if (hasInsideEnemy && hasOutsideEnemy)
        {
            spawnWeight = Mathf.Max(insideSpawnWeight, outsideSpawnWeight);
        }
        else if (hasInsideEnemy)
        {
            spawnWeight = insideSpawnWeight;
        }
        else if (hasOutsideEnemy)
        {
            spawnWeight = outsideSpawnWeight;
        }

        EnemyConfigDataDefault defaultConfigValues = new EnemyConfigDataDefault(spawnWeight, enemyType.MaxCount, hasInsideEnemy, hasOutsideEnemy);
        EnemyData enemyData = new EnemyData(level.PlanetName, defaultConfigValues);

        enemyData.BindConfigs();

        EnemyDataList.Add(enemyData);
    }

    public static EnemyData GetEnemyData(string planetName)
    {
        foreach (var enemyData in EnemyDataList)
        {
            if (enemyData.PlanetName == planetName)
            {
                return enemyData;
            }
        }

        return null;
    }

    public static bool HasEnemyData(string planetName)
    {
        return GetEnemyData(planetName) != null;
    }

    public static void SetEnemyDataForCurrentLevel()
    {
        EnemyData enemyData = GetEnemyData(LevelHelper.CurrentPlanetName);

        if (enemyData == null)
        {
            Plugin.Logger.LogError($"Failed to set enemy data for current level. EnemyData is null. (PlanetName: \"{LevelHelper.CurrentPlanetName}\")");
            return;
        }

        EnemyHelper.SetMaxSpawnCount(EnemyName, enemyData.ConfigData.MaxSpawnCount.Value, LevelHelper.CurrentPlanetName);
    }
}

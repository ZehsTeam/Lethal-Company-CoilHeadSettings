using HarmonyLib;
using System.Collections.Generic;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void Start()
    {
        SetCoilHeadSettings();
    }

    public static void SetCoilHeadSettings()
    {
        EnemyType enemyType = GetEnemyType("Spring");

        if (enemyType == null)
        {
            CoilHeadSettingsBase.mls.LogError("Error: could not find EnemyType \"Spring\".");
            return;
        }

        SyncedConfig configManager = CoilHeadSettingsBase.Instance.ConfigManager;

        enemyType.PowerLevel = configManager.PowerLevel;
        enemyType.MaxCount = configManager.MaxSpawned;

        CoilHeadSettingsBase.mls.LogInfo($"Successfully set Coil-Head settings.");
    }

    private static EnemyType GetEnemyType(string enemyName)
    {
        if (RoundManager.Instance == null)
        {
            CoilHeadSettingsBase.mls.LogError("Error: RoundManager does not exist yet.");
            return null;
        }

        List<SpawnableEnemyWithRarity> enemies = RoundManager.Instance.currentLevel.Enemies;

        foreach (var enemy in enemies)
        {
            EnemyType enemyType = enemy.enemyType;

            if (enemyType.enemyName == enemyName)
            {
                return enemyType;
            }
        }

        return null;
    }
}

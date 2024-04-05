using HarmonyLib;
using System.Collections.Generic;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    [HarmonyPatch("LoadNewLevel")]
    [HarmonyPostfix]
    static void LoadNewLevelPatch()
    {
        SetCoilHeadSettings();
    }

    [HarmonyPatch("GenerateNewLevelClientRpc")]
    [HarmonyPostfix]
    static void GenerateNewLevelClientRpcPatch()
    {
        if (Plugin.IsHostOrServer) return;

        SetCoilHeadSettings();
    }

    public static void SetCoilHeadSettings()
    {
        SelectableLevel currentLevel = RoundManager.Instance.currentLevel;
        SpawnableEnemyWithRarity spawnableEnemyWithRarity = GetSpawnableEnemyWithRarity("Spring");

        if (spawnableEnemyWithRarity == null)
        {
            Plugin.logger.LogError($"Error: could not find SpawnableEnemyWithRarity \"Spring\" in \"{currentLevel.PlanetName}\".");
            return;
        }

        SyncedConfig configManager = Plugin.Instance.ConfigManager;

        spawnableEnemyWithRarity.rarity = GetRarityForCoilHead(spawnableEnemyWithRarity);

        EnemyType enemyType = spawnableEnemyWithRarity.enemyType;

        enemyType.PowerLevel = configManager.PowerLevel;
        enemyType.MaxCount = configManager.MaxSpawnCount;

        Plugin.logger.LogInfo($"Successfully set Coil-Head settings for \"{currentLevel.PlanetName}\".");
    }

    private static SpawnableEnemyWithRarity GetSpawnableEnemyWithRarity(string enemyName)
    {
        return GetSpawnableEnemyWithRarity(RoundManager.Instance.currentLevel, enemyName);
    }

    private static SpawnableEnemyWithRarity GetSpawnableEnemyWithRarity(SelectableLevel level, string enemyName)
    {
        List<SpawnableEnemyWithRarity> enemies = level.Enemies;

        foreach (var enemy in enemies)
        {
            if (enemy.enemyType.enemyName == enemyName)
            {
                return enemy;
            }
        }

        return null;
    }

    private static int GetRarityForCoilHead(SpawnableEnemyWithRarity spawnableEnemyWithRarity)
    {
        SyncedConfig configManager = Plugin.Instance.ConfigManager;

        int rarity = spawnableEnemyWithRarity.rarity;

        SelectableLevel selectableLevel = RoundManager.Instance.currentLevel;
        string planetName = selectableLevel.PlanetName;

        switch (planetName)
        {
            case "44 Liquidation":
                rarity = configManager.LiquidationSpawnWeight;
                break;
            case "5 Embrion":
                rarity = configManager.EmbrionSpawnWeight;
                break;
            case "68 Artifice":
                rarity = configManager.ArtificeSpawnWeight;
                break;
            case "8 Titan":
                rarity = configManager.TitanSpawnWeight;
                break;
            case "7 Dine":
                rarity = configManager.DineSpawnWeight;
                break;
            case "85 Rend":
                rarity = configManager.RendSpawnWeight;
                break;
            case "20 Adamance":
                rarity = configManager.AdamanceSpawnWeight;
                break;
            case "21 Offense":
                rarity = configManager.OffenseSpawnWeight;
                break;
            case "61 March":
                rarity = configManager.MarchSpawnWeight;
                break;
            case "56 Vow":
                rarity = configManager.VowSpawnWeight;
                break;
        }

        return (int)(rarity * Plugin.Instance.ConfigManager.SpawnWeightMultiplier);
    }
}

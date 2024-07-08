using HarmonyLib;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal class RoundManagerPatch
{
    [HarmonyPatch("LoadNewLevel")]
    [HarmonyPostfix]
    static void LoadNewLevelPatch()
    {
        // Call on Host/Server
        SetCoilHeadSpawnDataForCurrentMoonOnServer();
    }

    private static void SetCoilHeadSpawnDataForCurrentMoonOnServer()
    {
        if (!Plugin.IsHostOrServer) return;

        SelectableLevel currentLevel = StartOfRound.Instance.currentLevel;
        SpawnData spawnData = SpawnDataManager.GetSpawnDataForCurrentMoon();

        if (spawnData == null)
        {
            Plugin.Instance.LogWarningExtended($"Warning: Failed to set Coil-Head spawn data for \"{currentLevel.PlanetName}\". SpawnData was not found.");
            return;
        }

        SpawnableEnemyWithRarity spawnableEnemyWithRarity = currentLevel.Enemies.Find(_ => _.enemyType.enemyName == "Spring");

        if (spawnableEnemyWithRarity == null)
        {
            Plugin.logger.LogError($"Error: Failed to set Coil-Head spawn data for \"{currentLevel.PlanetName}\". EnemyType \"Spring\" could not be found.");
            return;
        }

        spawnableEnemyWithRarity.enemyType.MaxCount = spawnData.MaxSpawnCount;
        spawnableEnemyWithRarity.rarity = spawnData.Rarity;

        Plugin.Instance.LogInfoExtended($"Set Coil-Head spawn data for \"{currentLevel.PlanetName}\". MaxSpawnCount: {spawnData.MaxSpawnCount}, Rarity: {spawnData.Rarity}");
    }
}

using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal class StartOfRoundPatch
{
    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    static void AwakePatch()
    {
        SpawnNetworkHandler();
    }

    private static void SpawnNetworkHandler()
    {
        if (!Plugin.IsHostOrServer) return;

        var networkHandlerHost = Object.Instantiate(Content.NetworkHandlerPrefab, Vector3.zero, Quaternion.identity);
        networkHandlerHost.GetComponent<NetworkObject>().Spawn();
    }

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch()
    {
        SetCoilHeadSettings();
    }

    [HarmonyPatch("OnClientConnect")]
    [HarmonyPrefix]
    static void OnClientConnectPatch(ref ulong clientId)
    {
        if (!Plugin.IsHostOrServer) return;

        ClientRpcParams clientRpcParams = new()
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = [clientId]
            }
        };

        Plugin.logger.LogInfo($"Sending config to client: {clientId}");

        PluginNetworkBehaviour.Instance.SendConfigToPlayerClientRpc(new SyncedConfigData(Plugin.ConfigManager), clientRpcParams);
    }

    [HarmonyPatch("OnLocalDisconnect")]
    [HarmonyPrefix]
    static void OnLocalDisconnectPatch()
    {
        Plugin.Instance.OnLocalDisconnect();
    }

    private static void SetCoilHeadSettings()
    {
        AddMissingCoilHeadEnemyToLevels();

        float powerLevel = Plugin.ConfigManager.PowerLevel.Value;

        foreach (var level in StartOfRound.Instance.levels)
        {
            if (!Utils.TryGetEnemyTypeInLevel(level, "Spring", out EnemyType enemyType))
            {
                continue;
            }

            enemyType.PowerLevel = powerLevel;
        }

        Plugin.Instance.LogInfoExtended($"Set Coil-Head PowerLevel to {powerLevel} for all moons.");
    }

    private static void AddMissingCoilHeadEnemyToLevels()
    {
        Plugin.Instance.LogInfoExtended("Adding missing Coil-Head enemy to levels.");

        if (!Utils.TryGetEnemyType("Spring", out EnemyType enemyType))
        {
            Plugin.logger.LogError("Error: Failed to add missing Coil-Head enemy to levels. EnemyType \"Spring\" could not be found.");
            return;
        }

        foreach (var moonSpawnData in SpawnDataManager.MoonSpawnDataList.List)
        {
            if (!Utils.TryGetLevelByPlanetName(moonSpawnData.PlanetName, out SelectableLevel level))
            {
                Plugin.logger.LogError($"Error: Failed to add missing Coil-Head enemy to \"{moonSpawnData.PlanetName}\". SelectableLevel is null.");
                continue;
            }

            if (Utils.GetEnemyTypeInLevel(level, "Spring") != null)
            {
                Plugin.Instance.LogInfoExtended($"\"{moonSpawnData.PlanetName}\" already has a Coil-Head enemy. Skipping.");
                continue;
            }

            SpawnableEnemyWithRarity spawnableEnemyWithRarity = new SpawnableEnemyWithRarity();
            spawnableEnemyWithRarity.enemyType = enemyType;
            spawnableEnemyWithRarity.rarity = moonSpawnData.Rarity;

            level.Enemies.Add(spawnableEnemyWithRarity);

            Plugin.Instance.LogInfoExtended($"Added missing Coil-Head enemy to \"{moonSpawnData.PlanetName}\". Rarity: {moonSpawnData.Rarity}");
        }

        Plugin.Instance.LogInfoExtended("Finished adding missing Coil-Head enemy to levels.");
    }
}

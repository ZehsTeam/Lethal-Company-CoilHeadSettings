using HarmonyLib;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal class StartOfRoundPatch
{
    private static AccessTools.FieldRef<EnemyType, int>? s_PowerLevelOld;
    private static AccessTools.FieldRef<EnemyType, float>? s_PowerLevelNew;

    public static void Initialize()
    {
        var powerLevelField = AccessTools.Field(typeof(EnemyType), nameof(EnemyType.PowerLevel));

        if (powerLevelField.FieldType == typeof(int))
        {
            s_PowerLevelOld = AccessTools.FieldRefAccess<EnemyType, int>(powerLevelField);
        }
        if (powerLevelField.FieldType == typeof(float))
        {
            s_PowerLevelNew = AccessTools.FieldRefAccess<EnemyType, float>(powerLevelField);
        }
    }

    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    static void AwakePatch()
    {
        SpawnNetworkHandler();
    }

    private static void SpawnNetworkHandler()
    {
        if (!Plugin.IsHostOrServer) return;

        var networkHandlerHost = Object.Instantiate(Content.networkHandlerPrefab, Vector3.zero, Quaternion.identity);
        networkHandlerHost.GetComponent<NetworkObject>().Spawn();
    }

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch()
    {
        //LogInfo();
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

        PluginNetworkBehaviour.Instance.SendConfigToPlayerClientRpc(new SyncedConfigData(Plugin.Instance.ConfigManager), clientRpcParams);
    }

    [HarmonyPatch("OnLocalDisconnect")]
    [HarmonyPrefix]
    static void OnLocalDisconnectPatch()
    {
        Plugin.Instance.OnLocalDisconnect();
    }

    public static void SetCoilHeadSettings()
    {
        foreach (var level in StartOfRound.Instance.levels)
        {
            SetCoilHeadSettingsForLevel(level);
        }
    }

    public static void SetCoilHeadSettingsForLevel(SelectableLevel level)
    {
        SpawnableEnemyWithRarity spawnableEnemyWithRarity = GetSpawnableEnemyWithRarity("Spring", level);

        if (spawnableEnemyWithRarity is null)
        {
            Plugin.logger.LogWarning($"Warning: could not find SpawnableEnemyWithRarity \"Spring\" in \"{level.PlanetName}\".");
            return;
        }

        if (TryGetCoilHeadRarityForLevel(level, out int newRarity))
        {
            spawnableEnemyWithRarity.rarity = newRarity;
        }

        EnemyType enemyType = spawnableEnemyWithRarity.enemyType;

        if (s_PowerLevelOld is not null)
        {
            s_PowerLevelOld(enemyType) = (int)Plugin.Instance.ConfigManager.PowerLevel;
        }
        if (s_PowerLevelNew is not null)
        {
            s_PowerLevelNew(enemyType) = (float)Plugin.Instance.ConfigManager.PowerLevel;
        }

        enemyType.MaxCount = Plugin.Instance.ConfigManager.MaxSpawnCount;

        Plugin.logger.LogInfo($"Successfully set Coil-Head settings for \"{level.PlanetName}\".");
    }

    private static SpawnableEnemyWithRarity GetSpawnableEnemyWithRarity(string enemyName, SelectableLevel level)
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

    private static bool TryGetCoilHeadRarityForLevel(SelectableLevel level, out int rarity)
    {
        rarity = 0;

        SyncedConfig configManager = Plugin.Instance.ConfigManager;

        switch (level.PlanetName)
        {
            case "44 Liquidation":
                rarity = configManager.LiquidationSpawnWeight;
                return true;
            case "5 Embrion":
                rarity = configManager.EmbrionSpawnWeight;
                return true;
            case "68 Artifice":
                rarity = configManager.ArtificeSpawnWeight;
                return true;
            case "8 Titan":
                rarity = configManager.TitanSpawnWeight;
                return true;
            case "7 Dine":
                rarity = configManager.DineSpawnWeight;
                return true;
            case "85 Rend":
                rarity = configManager.RendSpawnWeight;
                return true;
            case "20 Adamance":
                rarity = configManager.AdamanceSpawnWeight;
                return true;
            case "21 Offense":
                rarity = configManager.OffenseSpawnWeight;
                return true;
            case "61 March":
                rarity = configManager.MarchSpawnWeight;
                return true;
            case "56 Vow":
                rarity = configManager.VowSpawnWeight;
                return true;
        }

        return false;
    }

    static void LogInfo()
    {
        string message = "";

        foreach (var level in StartOfRound.Instance.levels)
        {
            message += GetMessageForLevel(level);
        }

        Plugin.logger.LogInfo($"\n\n{message.Trim()}\n");
    }

    static string GetMessageForLevel(SelectableLevel level)
    {
        string message = $"[{level.PlanetName}]".PadRight(25);

        SpawnableEnemyWithRarity spawnableEnemyWithRarity = level.Enemies.Find(_ => _.enemyType.enemyName == "Spring");
        if (spawnableEnemyWithRarity == null) return string.Empty;

        message += $" Spring vanilla spawnWeight: {spawnableEnemyWithRarity.rarity}\n";

        return message;
    }
}

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
        if (!CoilHeadSettingsBase.IsHostOrServer) return;

        var networkHandlerHost = Object.Instantiate(GameNetworkManagerPatch.networkPrefab, Vector3.zero, Quaternion.identity);
        networkHandlerHost.GetComponent<NetworkObject>().Spawn();
    }

    [HarmonyPatch("OnClientConnect")]
    [HarmonyPrefix]
    static void OnClientConnectPatch(ref ulong clientId)
    {
        if (!CoilHeadSettingsBase.IsHostOrServer) return;

        ClientRpcParams clientRpcParams = new()
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = [clientId]
            }
        };

        CoilHeadSettingsBase.mls.LogInfo($"Sending config to client: {clientId}");

        PluginNetworkBehaviour.Instance.SendConfigToPlayerClientRpc(new SyncedConfigData(CoilHeadSettingsBase.Instance.ConfigManager), clientRpcParams);
    }

    [HarmonyPatch("OnLocalDisconnect")]
    [HarmonyPrefix]
    static void OnLocalDisconnectPatch()
    {
        CoilHeadSettingsBase.Instance.OnLocalDisconnect();
    }
}

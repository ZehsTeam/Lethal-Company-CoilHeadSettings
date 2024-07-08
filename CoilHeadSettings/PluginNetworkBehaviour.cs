using Unity.Netcode;

namespace com.github.zehsteam.CoilHeadSettings;

internal class PluginNetworkBehaviour : NetworkBehaviour
{
    public static PluginNetworkBehaviour Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [ClientRpc]
    public void SendConfigToPlayerClientRpc(SyncedConfigData syncedConfigData, ClientRpcParams clientRpcParams = default)
    {
        if (Plugin.IsHostOrServer) return;

        Plugin.logger.LogInfo("Syncing config with host.");
        Plugin.ConfigManager.SetHostConfigData(syncedConfigData);
    }
}

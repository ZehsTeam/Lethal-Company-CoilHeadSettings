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
        if (CoilHeadSettingsBase.IsHostOrServer) return;

        CoilHeadSettingsBase.mls.LogInfo("Syncing config with host.");
        CoilHeadSettingsBase.Instance.ConfigManager.SetHostConfigData(syncedConfigData);
    }
}

using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(GameNetworkManager))]
internal class GameNetworkManagerPatch
{
    public static GameObject networkPrefab;

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void StartPatch()
    {
        LoadAssetsFromAssetBundle();
    }

    private static void LoadAssetsFromAssetBundle()
    {
        if (networkPrefab != null) return;

        try
        {
            var dllFolderPath = System.IO.Path.GetDirectoryName(CoilHeadSettingsBase.Instance.Info.Location);
            var assetBundleFilePath = System.IO.Path.Combine(dllFolderPath, "coilheadsettings_assets");
            AssetBundle MainAssetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);

            networkPrefab = MainAssetBundle.LoadAsset<GameObject>("NetworkHandler");
            networkPrefab.AddComponent<PluginNetworkBehaviour>();

            NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);

            CoilHeadSettingsBase.mls.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (System.Exception e)
        {
            CoilHeadSettingsBase.mls.LogError($"Error: failed to load assets from AssetBundle.\n\n{e}");
        }
    }
}

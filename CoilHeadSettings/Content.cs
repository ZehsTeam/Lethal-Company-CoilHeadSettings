using UnityEngine;

namespace com.github.zehsteam.CoilHeadSettings;

internal class Content
{
    // Network Handler
    public static GameObject NetworkHandlerPrefab;

    public static void Load()
    {
        LoadAssetsFromAssetBundle();
    }

    private static void LoadAssetsFromAssetBundle()
    {
        try
        {
            var dllFolderPath = System.IO.Path.GetDirectoryName(Plugin.Instance.Info.Location);
            var assetBundleFilePath = System.IO.Path.Combine(dllFolderPath, "coilheadsettings_assets");
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundleFilePath);

            // Network Handler
            NetworkHandlerPrefab = assetBundle.LoadAsset<GameObject>("NetworkHandler");
            NetworkHandlerPrefab.AddComponent<PluginNetworkBehaviour>();

            Plugin.logger.LogInfo("Successfully loaded assets from AssetBundle!");
        }
        catch (System.Exception e)
        {
            Plugin.logger.LogError($"Error: Failed to load assets from AssetBundle.\n\n{e}");
        }
    }
}

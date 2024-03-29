﻿using BepInEx;
using BepInEx.Logging;
using com.github.zehsteam.CoilHeadSettings.Patches;
using HarmonyLib;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace com.github.zehsteam.CoilHeadSettings;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class CoilHeadSettingsBase : BaseUnityPlugin
{
    private readonly Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    internal static CoilHeadSettingsBase Instance;
    internal static ManualLogSource mls;

    internal SyncedConfig ConfigManager;

    public static bool IsHostOrServer => NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;

    void Awake()
    {
        if (Instance == null) Instance = this;

        mls = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_GUID);
        mls.LogInfo($"{MyPluginInfo.PLUGIN_NAME} has awoken!");

        harmony.PatchAll(typeof(GameNetworkManagerPatch));
        harmony.PatchAll(typeof(StartOfRoundPatch));
        harmony.PatchAll(typeof(RoundManagerPatch));
        harmony.PatchAll(typeof(SpringManAIPatch));

        ConfigManager = new SyncedConfig();

        NetcodePatcherAwake();
    }

    private void NetcodePatcherAwake()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();

        foreach (var type in types)
        {
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);

                if (attributes.Length > 0)
                {
                    method.Invoke(null, null);
                }
            }
        }
    }

    public void OnLocalDisconnect()
    {
        mls.LogInfo($"Local player disconnected. Removing hostConfigData.");
        ConfigManager.SetHostConfigData(null);
    }
}

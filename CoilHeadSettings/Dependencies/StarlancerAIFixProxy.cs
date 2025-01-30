using BepInEx.Bootstrap;

namespace com.github.zehsteam.CoilHeadSettings.Dependencies;

internal static class StarlancerAIFixProxy
{
    public const string PLUGIN_GUID = "AudioKnight.StarlancerAIFix";
    public static bool Enabled
    {
        get
        {
            _enabled ??= Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID);
            return _enabled.Value;
        }
    }

    private static bool? _enabled;
}

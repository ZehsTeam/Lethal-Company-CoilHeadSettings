using BepInEx.Bootstrap;

namespace com.github.zehsteam.CoilHeadSettings.Dependencies;

internal static class StarlancerAIFixProxy
{
    public const string PLUGIN_GUID = "AudioKnight.StarlancerAIFix";
    public static bool Enabled => Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID);
}

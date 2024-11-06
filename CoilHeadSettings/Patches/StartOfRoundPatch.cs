using HarmonyLib;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(StartOfRound))]
internal static class StartOfRoundPatch
{
    [HarmonyPatch(nameof(StartOfRound.Start))]
    [HarmonyPostfix]
    private static void StartPatch()
    {
        EnemyDataManager.Initialize();
    }
}

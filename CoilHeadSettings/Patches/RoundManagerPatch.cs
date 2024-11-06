using HarmonyLib;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(RoundManager))]
internal static class RoundManagerPatch
{
    [HarmonyPatch(nameof(RoundManager.LoadNewLevel))]
    [HarmonyPostfix]
    private static void LoadNewLevelPatch()
    {
        EnemyDataManager.SetEnemyDataForCurrentLevel();
    }
}

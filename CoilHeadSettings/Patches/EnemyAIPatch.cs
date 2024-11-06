using HarmonyLib;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal static class EnemyAIPatch
{
    [HarmonyPatch(nameof(EnemyAI.Start))]
    [HarmonyPostfix]
    private static void StartPatch(ref EnemyAI __instance)
    {
        SpringManAI springManAI = __instance as SpringManAI;

        if (springManAI != null)
        {
            SpringManAIPatch.Start(springManAI);
        }
    }
}

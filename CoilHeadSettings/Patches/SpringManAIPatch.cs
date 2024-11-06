using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(SpringManAI))]
internal static class SpringManAIPatch
{
    private static int _attackDamage => Plugin.ConfigManager.Enemy_AttackDamage.Value;
    private static float _hitPlayerTimer => 1f / Plugin.ConfigManager.Enemy_AttackSpeed.Value;

    public static void Start(SpringManAI springManAI)
    {
        if (springManAI == null)
        {
            return;
        }

        springManAI.currentChaseSpeed = Plugin.ConfigManager.Enemy_MovementSpeed.Value;
    }
    
    [HarmonyPatch(nameof(SpringManAI.OnCollideWithPlayer))]
    [HarmonyPrefix]
    private static bool OnCollideWithPlayerPatch(ref SpringManAI __instance, Collider other)
    {
        if (!__instance.stoppingMovement && __instance.currentBehaviourStateIndex == 1 && !(__instance.hitPlayerTimer >= 0f) && !__instance.setOnCooldown && !((double)(Time.realtimeSinceStartup - __instance.timeAtLastCooldown) < 0.45))
        {
            PlayerControllerB playerControllerB = __instance.MeetsStandardPlayerCollisionConditions(other);

            if (playerControllerB != null)
            {
                __instance.hitPlayerTimer = _hitPlayerTimer;
                playerControllerB.DamagePlayer(_attackDamage, hasDamageSFX: true, callRPC: true, CauseOfDeath.Mauling, 2);
                playerControllerB.JumpToFearLevel(1f);
                __instance.timeSinceHittingPlayer = Time.realtimeSinceStartup;
            }
        }

        return false;
    }

    public static void SettingsChanged()
    {
        foreach (var springManAI in Object.FindObjectsByType<SpringManAI>(FindObjectsSortMode.None))
        {
            springManAI.currentChaseSpeed = Plugin.ConfigManager.Enemy_MovementSpeed.Value;
        }
    }
}

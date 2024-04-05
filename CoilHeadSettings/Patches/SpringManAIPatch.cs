using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(SpringManAI))]
internal class SpringManAIPatch
{
    [HarmonyPatch("__initializeVariables")]
    [HarmonyPostfix]
    static void __initializeVariablesPatch(ref float ___currentChaseSpeed)
    {
        ___currentChaseSpeed = Plugin.Instance.ConfigManager.MovementSpeed;
    }

    [HarmonyPatch("OnCollideWithPlayer")]
    [HarmonyPrefix]
    static bool OnCollideWithPlayerPatch(ref SpringManAI __instance, Collider other, ref bool ___stoppingMovement, ref float ___timeSinceHittingPlayer)
    {
        __instance.OnCollideWithEnemy(other);

        if (!___stoppingMovement && __instance.currentBehaviourStateIndex == 1 && !(___timeSinceHittingPlayer >= 0f))
        {
            PlayerControllerB playerControllerB = __instance.MeetsStandardPlayerCollisionConditions(other);

            if (playerControllerB != null)
            {
                SyncedConfig configManager = Plugin.Instance.ConfigManager;

                int attackDamage = configManager.AttackDamage;
                float attackSpeed = configManager.AttackSpeed;

                ___timeSinceHittingPlayer = 1f / attackSpeed;
                playerControllerB.DamagePlayer(attackDamage, hasDamageSFX: true, callRPC: true, CauseOfDeath.Mauling, 2);
                playerControllerB.JumpToFearLevel(1f);
            }
        }

        return false;
    }
}

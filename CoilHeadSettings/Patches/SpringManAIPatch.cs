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
        ___currentChaseSpeed = Plugin.ConfigManager.MovementSpeed.Value;
    }

    [HarmonyPatch("OnCollideWithPlayer")]
    [HarmonyPrefix]
    static bool OnCollideWithPlayerPatch(ref SpringManAI __instance, Collider other, ref bool ___stoppingMovement, ref float ___timeSinceHittingPlayer)
    {
        __instance.OnCollideWithEnemy(other);

        if (!___stoppingMovement && __instance.currentBehaviourStateIndex == 1 && ___timeSinceHittingPlayer < 0f)
        {
            PlayerControllerB playerScript = __instance.MeetsStandardPlayerCollisionConditions(other);
            if (playerScript == null) return false;

            int attackDamage = Plugin.ConfigManager.AttackDamage.Value;
            float attackSpeed = Plugin.ConfigManager.AttackSpeed.Value;

            ___timeSinceHittingPlayer = 1f / attackSpeed;
            playerScript.DamagePlayer(attackDamage, hasDamageSFX: true, callRPC: true, causeOfDeath: CauseOfDeath.Mauling, deathAnimation: 2);
            playerScript.JumpToFearLevel(1f);
        }

        return false;
    }
}

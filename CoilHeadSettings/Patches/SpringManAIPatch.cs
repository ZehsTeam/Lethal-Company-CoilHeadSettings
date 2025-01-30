using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace com.github.zehsteam.CoilHeadSettings.Patches;

[HarmonyPatch(typeof(SpringManAI))]
internal static class SpringManAIPatch
{
    public static int AttackDamage => Plugin.ConfigManager.Enemy_AttackDamage.Value;
    public static float HitPlayerTimer => 1f / Plugin.ConfigManager.Enemy_AttackSpeed.Value;

    public static void Start(SpringManAI springManAI)
    {
        if (springManAI == null) return;

        springManAI.currentChaseSpeed = Plugin.ConfigManager.Enemy_MovementSpeed.Value;
    }

    [HarmonyPatch(nameof(SpringManAI.OnCollideWithPlayer))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> OnCollideWithPlayerTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        MethodInfo attackDamageGetter = typeof(SpringManAIPatch).GetProperty(nameof(AttackDamage))?.GetGetMethod();
        MethodInfo hitPlayerTimerGetter = typeof(SpringManAIPatch).GetProperty(nameof(HitPlayerTimer))?.GetGetMethod();

        if (attackDamageGetter == null || hitPlayerTimerGetter == null)
        {
            Plugin.Logger.LogError("Failed to retrieve getter methods for AttackDamage or HitPlayerTimer.");
            return instructions;
        }

        var code = new List<CodeInstruction>(instructions);

        for (int i = 0; i < code.Count; i++)
        {
            if (code[i].opcode == OpCodes.Ldc_I4_S && code[i].operand is sbyte number && number == 90)
            {
                code[i] = new CodeInstruction(OpCodes.Call, attackDamageGetter);
            }
            else if (code[i].opcode == OpCodes.Ldc_R4 && code[i].operand is float number2 && number2 == 0.2f)
            {
                code[i] = new CodeInstruction(OpCodes.Call, hitPlayerTimerGetter);
            }
        }

        return code.AsEnumerable();
    }

    public static void OnConfigSettingsChanged()
    {
        if (RoundManager.Instance == null || RoundManager.Instance.SpawnedEnemies == null)
        {
            return;
        }

        foreach (var enemyAI in RoundManager.Instance.SpawnedEnemies)
        {
            if (enemyAI is not SpringManAI springManAI)
            {
                continue;
            }

            springManAI.currentChaseSpeed = Plugin.ConfigManager.Enemy_MovementSpeed.Value;
        }
    }
}

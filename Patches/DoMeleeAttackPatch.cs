using HarmonyLib;

namespace TieredToolWear.Patches
{
    [HarmonyPatch(typeof(Attack), nameof(Attack.DoMeleeAttack))]
    public static class DoMeleeAttackPatch
    {
        static void Prefix(Attack __instance)
        {
            ItemDrop.ItemData weapon = __instance.m_weapon;
            if (weapon == null) return;

            DurabilityTracker.BeginSwing(weapon);
        }

        static void Postfix(Attack __instance)
        {
            DurabilityTracker.ResolveSwing(__instance.m_weapon);
        }
    }
}

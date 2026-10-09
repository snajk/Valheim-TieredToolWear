using HarmonyLib;

namespace TieredToolWear.Patches
{
    [HarmonyPatch(typeof(Destructible), nameof(Destructible.Damage))]
    public static class DestructiblePatch
    {
        static void Postfix(Destructible __instance, HitData hit)
        {
            DurabilityTracker.RecordHit(hit, __instance.m_minToolTier, $"Destructible ({__instance.name})");
        }
    }
}

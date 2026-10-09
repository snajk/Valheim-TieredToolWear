using HarmonyLib;

namespace TieredToolWear.Patches
{
    [HarmonyPatch(typeof(TreeLog), nameof(TreeLog.Damage))]
    public static class TreeLogPatch
    {
        static void Postfix(TreeLog __instance, HitData hit)
        {
            DurabilityTracker.RecordHit(hit, __instance.m_minToolTier, $"Log ({__instance.name})");
        }
    }
}

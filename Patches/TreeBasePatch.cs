using HarmonyLib;

namespace TieredToolWear.Patches
{
    [HarmonyPatch(typeof(TreeBase), nameof(TreeBase.Damage))]
    public static class TreeBasePatch
    {
        static void Postfix(TreeBase __instance, HitData hit)
        {
            DurabilityTracker.RecordHit(hit, __instance.m_minToolTier, $"Tree ({__instance.name})");
        }
    }
}

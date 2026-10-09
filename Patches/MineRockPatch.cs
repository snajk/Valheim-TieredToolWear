using HarmonyLib;

namespace TieredToolWear.Patches
{
    [HarmonyPatch(typeof(MineRock), nameof(MineRock.Damage))]
    public static class MineRockPatch
    {
        static void Postfix(MineRock __instance, HitData hit)
        {
            DurabilityTracker.RecordHit(hit, __instance.m_minToolTier, $"Rock ({__instance.name})");
        }
    }
}

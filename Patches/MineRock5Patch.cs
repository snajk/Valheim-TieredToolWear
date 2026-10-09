using HarmonyLib;

namespace TieredToolWear.Patches
{
    [HarmonyPatch(typeof(MineRock5), nameof(MineRock5.Damage))]
    public static class MineRock5Patch
    {
        static void Postfix(MineRock5 __instance, HitData hit)
        {
            DurabilityTracker.RecordHit(hit, __instance.m_minToolTier, $"Rock5 ({__instance.name})");
        }
    }
}

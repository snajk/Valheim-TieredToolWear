using HarmonyLib;

namespace TieredToolWear.Patches
{
    [HarmonyPatch(typeof(WearNTear), nameof(WearNTear.Damage))]
    public static class WearNTearPatch
    {
        static void Postfix(WearNTear __instance, HitData hit)
        {
            DurabilityTracker.RecordHit(hit, __instance.m_minToolTier, $"Building ({__instance.name})");
        }
    }
}

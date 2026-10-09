using HarmonyLib;

namespace TieredToolWear.Patches
{
    [HarmonyPatch(typeof(Attack), nameof(Attack.SpawnOnHitTerrain))]
    public static class GroundHitPatch
    {
        static void Postfix(Character character, ItemDrop.ItemData weapon)
        {
            if (!(character is Player)) return;
            if (weapon == null || !DurabilityTracker.IsTrackedTool(weapon)) return;

            DurabilityTracker.RecordHit(weapon, targetMinToolTier: 0, targetName: "Ground/Dirt Hit");
        }
    }
}

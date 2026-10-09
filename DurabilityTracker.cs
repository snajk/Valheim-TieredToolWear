using BepInEx.Logging;
using UnityEngine;

namespace TieredToolWear
{
    internal static class DurabilityTracker
    {
        internal static ManualLogSource Logger;

        private static ItemDrop.ItemData s_pendingWeapon;
        private static float s_pendingDurabilityBeforeSwing;
        private static bool s_pendingHit;
        private static int s_pendingTargetMinToolTier;
        private static string s_pendingTargetName;

        internal static void BeginSwing(ItemDrop.ItemData weapon)
        {
            s_pendingWeapon = weapon;
            s_pendingDurabilityBeforeSwing = weapon.m_durability;
            s_pendingHit = false;
        }

        internal static void ResolveSwing(ItemDrop.ItemData weapon)
        {
            if (weapon == null || weapon != s_pendingWeapon) return;

            if (!s_pendingHit)
            {
                s_pendingWeapon = null;
                return;
            }

            int rawTier = weapon.m_shared.m_toolTier;
            int effectiveTier = ToEffectiveTier(weapon);
            int tierDiff = effectiveTier - s_pendingTargetMinToolTier;

            float finalDrain;
            if (tierDiff > 0)
            {
                float reduction = Mathf.Clamp01(tierDiff * PluginConfig.DurabilityReductionPerTier.Value);
                finalDrain = PluginConfig.BaseDurabilityLossPerHit.Value * (1f - reduction);
            }
            else
            {
                finalDrain = PluginConfig.BaseDurabilityLossPerHit.Value;
            }

            float beforeSwing = s_pendingDurabilityBeforeSwing;
            weapon.m_durability = Mathf.Min(beforeSwing - finalDrain, beforeSwing);

            if (PluginConfig.EnableDebugLogging.Value)
            {
                Logger.LogInfo($"[{s_pendingTargetName}] {weapon.m_shared.m_name} (Tier {rawTier}, Effective {effectiveTier}) " +
                    $"vs Target Tier {s_pendingTargetMinToolTier} | TierDiff {tierDiff} | Drain {finalDrain:F2} | " +
                    $"Durability {beforeSwing:F1} -> {weapon.m_durability:F1}");
            }

            s_pendingWeapon = null;
        }

        internal static void RecordHit(HitData hit, int targetMinToolTier, string targetName)
        {
            Player attacker = hit.GetAttacker() as Player;
            if (attacker == null) return;

            ItemDrop.ItemData tool = attacker.GetRightItem();
            if (tool == null || !IsTrackedTool(tool)) return;

            RecordHit(tool, targetMinToolTier, targetName);
        }

        internal static void RecordHit(ItemDrop.ItemData tool, int targetMinToolTier, string targetName)
        {
            if (tool != s_pendingWeapon) return;

            if (!s_pendingHit || targetMinToolTier < s_pendingTargetMinToolTier)
            {
                s_pendingHit = true;
                s_pendingTargetMinToolTier = targetMinToolTier;
                s_pendingTargetName = targetName;
            }
        }

        internal static bool IsTrackedTool(ItemDrop.ItemData item)
        {
            global::Skills.SkillType skill = item.m_shared.m_skillType;
            if (skill == global::Skills.SkillType.Pickaxes) return PluginConfig.EnablePickaxeDurabilityScaling.Value;
            if (skill == global::Skills.SkillType.Axes) return PluginConfig.EnableAxeDurabilityScaling.Value;
            return false;
        }

        // Axes report a tool tier one higher than pickaxes for the equivalent material level
        // (Iron Axe = tier 3, Iron Pickaxe = tier 2), so axes are normalized down by 1.
        private static int ToEffectiveTier(ItemDrop.ItemData weapon)
        {
            int rawTier = weapon.m_shared.m_toolTier;
            return weapon.m_shared.m_skillType == global::Skills.SkillType.Axes ? rawTier - 1 : rawTier;
        }
    }
}

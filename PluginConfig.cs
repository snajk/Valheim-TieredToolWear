using BepInEx.Configuration;

namespace TieredToolWear
{
    internal static class PluginConfig
    {
        internal static ConfigEntry<float> DurabilityReductionPerTier;
        internal static ConfigEntry<float> BaseDurabilityLossPerHit;
        internal static ConfigEntry<bool> EnablePickaxeDurabilityScaling;
        internal static ConfigEntry<bool> EnableAxeDurabilityScaling;
        internal static ConfigEntry<bool> EnableDebugLogging;

        internal static void Bind(ConfigFile config)
        {
            DurabilityReductionPerTier = config.Bind(
                "General",
                "DurabilityReductionPerTier",
                0.3f,
                "Fraction of durability loss reduced per tier the tool is above the material's tier. " +
                "0.3 = 30% less wear per tier of advantage. Clamped to [0, 1].");

            BaseDurabilityLossPerHit = config.Bind(
                "General",
                "BaseDurabilityLossPerHit",
                1.0f,
                "Baseline durability lost per hit, replacing the base game's own durability calculation.");

            EnablePickaxeDurabilityScaling = config.Bind(
                "General",
                "EnablePickaxeDurabilityScaling",
                true,
                "Enable dynamic durability scaling for pickaxes (ore, rock, dirt).");

            EnableAxeDurabilityScaling = config.Bind(
                "General",
                "EnableAxeDurabilityScaling",
                true,
                "Enable dynamic durability scaling for axes (trees, logs, player-built structures).");

            EnableDebugLogging = config.Bind(
                "Debug",
                "EnableDebugLogging",
                false,
                "Log per-hit tool/tier/durability details to the BepInEx console.");
        }
    }
}

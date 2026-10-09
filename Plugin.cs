using BepInEx;
using HarmonyLib;
using TieredToolWear.Patches;

namespace TieredToolWear
{
    [BepInPlugin("io.github.snajk.tieredtoolwear", "Tiered Tool Wear (Beta)", "0.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            DurabilityTracker.Logger = Logger;
            PluginConfig.Bind(Config);

            Logger.LogInfo("Tiered Tool Wear initialized!");

            Harmony.CreateAndPatchAll(typeof(DoMeleeAttackPatch));
            Harmony.CreateAndPatchAll(typeof(MineRock5Patch));
            Harmony.CreateAndPatchAll(typeof(MineRockPatch));
            Harmony.CreateAndPatchAll(typeof(GroundHitPatch));
            Harmony.CreateAndPatchAll(typeof(DestructiblePatch));
            Harmony.CreateAndPatchAll(typeof(TreeBasePatch));
            Harmony.CreateAndPatchAll(typeof(TreeLogPatch));
            Harmony.CreateAndPatchAll(typeof(WearNTearPatch));
        }
    }
}


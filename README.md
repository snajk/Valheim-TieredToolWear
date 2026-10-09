# Tiered Tool Wear

> **Beta** — core functionality is working but this mod hasn't seen extensive testing across all biomes/materials yet. Back up your save and report issues.

A [BepInEx](https://github.com/BepInEx/BepInEx)/[Harmony](https://github.com/pardeike/Harmony) mod for Valheim that replaces the vanilla durability-loss calculation for pickaxes and axes with a tier-aware one: using a tool on a material well below its tier causes less wear, instead of the flat per-hit durability loss the base game applies regardless of material.

## What it does

Vanilla Valheim deducts a fixed amount of durability per hit regardless of how "overkill" your tool is for the material. This mod intercepts every melee swing (`Attack.DoMeleeAttack`) and, after the swing resolves, overwrites the vanilla durability result with a custom calculation based on the lowest-tier target hit during that swing:

- If your tool's tier is **at or below** the target material's required tool tier, durability loss is unchanged (`BaseDurabilityLossPerHit`).
- If your tool's tier is **above** the target material's tier, durability loss is reduced by `DurabilityReductionPerTier` for every tier of advantage, clamped so wear reduction can never reach 100%.

This applies to:

| Target type | Patch |
|---|---|
| Ore veins, Black Marble, Muddy Scrap | `MineRock5Patch` |
| Boulders / surface rocks | `MineRockPatch` |
| Ground/dirt (terrain hits) | `GroundHitPatch` |
| Small destructible rocks/props | `DestructiblePatch` |
| Standing trees | `TreeBasePatch` |
| Felled logs | `TreeLogPatch` |
| Player-built structures (walls, etc.) | `WearNTearPatch` |

A single swing can register hits against multiple targets (e.g. splash damage); the lowest target tier hit is used for that swing's durability calculation, matching the "least wear for the easiest material hit" intent of the mod.

### Axe/pickaxe tier normalization

Axes report a tool tier one higher than pickaxes for the equivalent material level (e.g. Iron Axe = tier 3, Iron Pickaxe = tier 2 for the same "Iron-tier" materials). To compare fairly against the same target tiers, axe tool tiers are normalized down by 1 internally before the tier-diff calculation. This offset is currently hardcoded and not configurable.

## Configuration

Config file: `BepInEx/config/io.github.snajk.tieredtoolwear.cfg` (generated on first run).

| Section | Key | Default | Description |
|---|---|---|---|
| General | `DurabilityReductionPerTier` | `0.3` | Fraction of durability loss reduced per tier the tool is above the material's tier. `0.3` = 30% less wear per tier of advantage. Clamped to `[0, 1]`. |
| General | `BaseDurabilityLossPerHit` | `1.0` | Baseline durability lost per hit, replacing the base game's own calculation entirely. |
| General | `EnablePickaxeDurabilityScaling` | `true` | Enable scaling for pickaxes (ore, rock, dirt). Disable to restore fully vanilla pickaxe durability behavior. |
| General | `EnableAxeDurabilityScaling` | `true` | Enable scaling for axes (trees, logs, player-built structures). Disable to restore fully vanilla axe durability behavior. |
| Debug | `EnableDebugLogging` | `false` | Log per-hit tool/tier/durability details to the BepInEx console. Useful for tuning the config values above. |

> **Note:** This mod was previously published under the GUID `com.custom.pickaxedurability` (as "Dynamic Pickaxe Durability"). Because the GUID has changed to `io.github.snajk.tieredtoolwear`, BepInEx will **not** carry over old config values automatically — a fresh config file is generated, and any previously tuned values need to be re-entered manually.

## Building from source

Requires the .NET SDK and a local Valheim install (for the game's managed assemblies).

```bash
dotnet build
```

By default this only compiles the mod to `bin/Debug/netstandard2.1/TieredToolWear.dll` — it does **not** copy the DLL into your Valheim plugins folder.

### Game path

The project assumes a default Steam install path for Linux (`~/.local/share/Steam/steamapps/common/Valheim`). If your game lives elsewhere (Windows, macOS, a custom Steam library, a non-Steam copy, etc.), override it on the command line instead of editing the `.csproj`:

```bash
dotnet build -p:ValheimPath="C:\Program Files (x86)\Steam\steamapps\common\Valheim"
```

### Auto-copy to plugins folder

Pass `-p:CopyToPlugins=true` to automatically copy the built DLL into `$(ValheimPath)/BepInEx/plugins/` after a successful build:

```bash
dotnet build -p:CopyToPlugins=true -p:ValheimPath="/path/to/Valheim"
```

This is off by default so a plain `dotnet build` works for anyone cloning the repo, regardless of whether they even have Valheim/BepInEx installed locally.

## Installation (pre-built release)

1. Install [BepInEx](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) for Valheim if you haven't already.
2. Drop `TieredToolWear.dll` into `BepInEx/plugins/`.
3. Launch the game once to generate the default config file, then tune values as desired under `BepInEx/config/io.github.snajk.tieredtoolwear.cfg`.

## Project layout

- `Plugin.cs` — BepInEx entry point, `Awake()` wiring config + Harmony patches.
- `PluginConfig.cs` — all `ConfigEntry<T>` fields and binding.
- `DurabilityTracker.cs` — per-swing state tracking and the core durability override logic.
- `Patches/` — one Harmony patch class per vanilla hook point.

## License

This project is licensed under the [GNU General Public License v3.0](LICENSE) (GPL-3.0). Forks and derivative works — including commercial ones — are permitted, but must remain open-source under the same license (copyleft).

This mod depends on [BepInEx](https://github.com/BepInEx/BepInEx) (LGPL-2.1) and [HarmonyLib](https://github.com/pardeike/Harmony) (MIT) at runtime; both are compatible with GPL-3.0 as dependencies and require no action from users or forkers.

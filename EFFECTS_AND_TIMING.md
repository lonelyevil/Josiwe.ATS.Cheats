# Gameplay Effects and Activation Timing

This document describes the gameplay effects exposed by the current mod and when a changed value actually reaches gameplay.

Changing a value in the F1 ConfigurationManager window updates the mod configuration immediately, but that does **not** necessarily regenerate existing game state. Many patches run only during a later event such as embarking, generating a reward, starting a season, or setting up a camera.

## Timing terms

- **Read-time / live:** The new value is used the next time the game calls the patched calculation. The current tick, UI, or already cached object may refresh later.
- **Next event:** The value is consumed only when the named event occurs, such as a new year, a newcomer group, or a reward decline.
- **Next embark / new settlement:** The value affects a future embark or settlement. It does not retroactively rewrite the current one.
- **Next setup / load:** The value is used the next time the relevant controller or service is initialized. A reload, scene transition, or new game may be required.
- **Next placement / construction:** The value affects newly created objects. Existing objects are not necessarily rebuilt or replaced.

## Configuration, persistence, and presets

- Without a main JSON file, the BepInEx `.cfg` is authoritative.
- If `Josiwe.ATS.Cheats.Config.json` exists beside the DLL, it is loaded at plugin startup and copied into BepInEx. Later panel edits are written back to that JSON while preserving unknown heading and description properties.
- The **Presets → Apply preset** dropdown applies a complete preset to the original `CheatConfig` settings immediately. Each affected cheat still follows the activation timing listed below.
- Presets do **not** include or reset the standalone **EffectsState Multipliers**, **Season Traits**, **Farm**, **House**, **Newcomers**, **Hostility**, or **Embark** settings. Those values remain unchanged when a preset is applied.
- Applying a preset does not reroll current seasonal traits, regenerate existing rewards, rebuild farms, or rewrite already generated caravans.

## World map, camera, and embark settings

| Setting | Effect | When it takes effect |
|---|---|---|
| `ZoomMultiplier` | Changes maximum zoom-out and adjusts zoom smoothing. | **Next `CameraController.SetUp`**, normally a map/scene load, reload, or new settlement. Not live for an already initialized camera. |
| `AllRacesInWorldMap` | Replaces vanilla revealed-race generation with the mod's random 1–4 generation path. | **Next caravan generation** on the world map. Existing caravans are unchanged. |
| `BonusPreparationPoints` (`0–100`) | Adds the value to the original bonus preparation points. Base points and difficulty penalties remain intact. | **Next embark/preparation-point calculation**, normally when the embark screen reads it. |
| `Embark / Goods amount multiplier` (`1–50`) | Multiplies the original embark-goods result after MetaPerks bonuses. Affects caravan goods and embark-bonus goods. | **Next embark-goods generation.** Existing settlement inventories are unchanged. |

## Reputation, impatience, blueprints, and cornerstones

| Setting | Effect | When it takes effect |
|---|---|---|
| `WildcardBlueprints` | Replaces the initial building picks with wildcard picks and replaces future reputation-threshold blueprint rewards with wildcard picks. | Initial replacement is **new settlement only**. Future threshold rewards change at the **next reputation reward generation**. Existing picks are not replaced. |
| `BlueprintsMultiplier` | Sets the number of wildcard picks granted by the initial replacement. | **New settlement only**, when initial reputation picks are prepared. It is not the count for every later threshold reward. |
| `InfiniteCornerstoneRerolls` | Sets the current cornerstone reroll count to 99 whenever the game reads it. | **Next reroll-count read**, effectively live for UI and reroll checks. This writes `CornerstonesState.rerollsLeft`, so it can enter save data. |
| `ResolveMultiplier` | Multiplies resolve-generated reputation before passing it to the reputation service. | **Next resolve reputation gain.** Resolve gains are also processed by `ReputationMutiplier`, so the two settings can compound. |
| `ReputationMutiplier` | Multiplies reputation additions and applies the configured cap/stopgap behavior. The spelling is a shipped compatibility key. | **Next reputation addition.** |
| `ImpatienceMultiplier` | Multiplies impatience additions and applies the configured cap/stopgap behavior. | **Next impatience addition.** |
| `ReputationStopgap` | Caps resolve-based reputation below the win threshold; orders and map events can still win. | **Next reputation calculation/addition.** |
| `ImpatienceStopgap` | Caps impatience below the loss threshold. | **Next impatience calculation/addition.** |
| `MoarMaxReputation` | Adds to the reputation required to win. | **Next win-threshold read**, such as UI or win checks. |
| `MoarMaxImpatience` | Adds to the impatience threshold that causes a loss. | **Next loss-threshold read.** |
| `MoarSeasonRewards` | Raises cornerstone option generation toward four options and removes the normal exclusion list, allowing repeats. | **Next cornerstone reward generation.** It also increases persistent meta `bonusSeasonRewardsAmount`; disabling the setting does not revert that profile-side effect. |
| `CornerstonePicksPerSeason` | Generates additional cornerstone picks so the original plus added picks reach the requested count. | **Next seasonal cornerstone-pick generation.** Existing waiting picks remain. |
| `CashRewardMultiplier` | Multiplies goods received for declining a cornerstone. `0` means no decline reward. | **Next cornerstone decline.** |
| `EnableAllBuildingsMoving` | Enables moving most buildings through the game controller hook. | **Next `GameController.StartGame`**, normally a new game or reload. Enabling mid-game is not retroactive. Disabling does not revoke an already enabled mover state. |

## Season length settings

| Setting | Effect | When it takes effect |
|---|---|---|
| `DrizzleLengthMultiplier` | Multiplies the original Drizzle duration rate. | **Next season-duration calculation.** Assume the current season keeps its established end time; the next transition is the safe expectation. |
| `ClearanceLengthMultiplier` | Same for Clearance. | Same as above. |
| `StormLengthMultiplier` | Same for Storm. | Same as above. |

The separate EffectsState season controls below are applied at a different layer. They multiply the final duration after these CheatConfig season multipliers, rather than replacing them.

## Prestige / ascension settings

These settings modify ascension effect models while initial effects are being built for embark. They are **next embark / new settlement** settings. They do not retroactively rewrite an active settlement, and loading an existing save does not rerun the generation path. Custom-game and tutorial branches can intentionally bypass parts of this logic.

| Setting | Modified effect |
|---|---|
| `Prestige_2_Amount` | Storm-season length modifier. |
| `Prestige_4_Amount` | Blueprint reroll cost. |
| `Prestige_5_Amount` | Villager leaving rate. |
| `Prestige_6_Amount` | Construction material cost. |
| `Prestige_7_Amount` | Chance of extra food consumption from Parasites. |
| `Prestige_8_Amount` | Chance of extra luxury consumption. |
| `Prestige_9_Amount` | Relic/glade-event working rate. |
| `Prestige_10_Amount` | Global trader sell-price modifier. |
| `Prestige_12_Amount` | Initial blueprint reward options; this is handled in the early pre-game modifier pass. |
| `Prestige_13_Amount` | Cornerstone reward options. |
| `Prestige_14_Amount` | Impatience reduction gained per reputation. |
| `Prestige_15_Amount` | Global resolve/reputation threshold increase. |
| `Prestige_17_Amount` | Hunger stacks applied when villagers do not eat. |
| `Prestige_18_Amount` | Hearth sacrifice time/consumption modifier. |

## EffectsState multipliers

All 26 controls are independent range entries from `0` to `10`, with `1` as neutral. Except for the temporary New Year guard described below, they multiply values at Harmony read points and do not write the configured factor into `EffectsState`.

“Live” below means the next relevant calculation, not necessarily an immediate redraw or reconstruction of an object that already cached the result.

| Setting | Effect | When it takes effect |
|---|---|---|
| `plantingSpeed` | Multiplies farm planting rate. | **Next planting-rate read**, normally a planting action/update. |
| `harvestingSpeed` | Multiplies farm harvesting rate. | **Next harvesting-rate read.** |
| `fuelConsumptionSpeed` | Multiplies fuel consumption speed; higher values burn fuel faster. | **Next fuel/burning-time calculation.** |
| `hearthSacraficeTimeRate` | Multiplies the hearth sacrifice rate. Higher values make sacrifice consumption slower in the original formula. | **Next hearth-sacrifice rate read.** |
| `constructionSpeed` | Multiplies construction progress. | **Next construction-progress update.** |
| `constructionCost` | Multiplies calculated construction cost. | **Next cost calculation/placement.** Already placed construction may retain previously calculated costs. |
| `relicsWorkingTimeRate` | Multiplies relic/glade-event working speed. | **Next relic working-time calculation.** |
| `grassAmountRatio` | Multiplies and rounds generated grass amount. | **Next grass calculation**, commonly map/glade generation rather than continuous terrain regeneration. |
| `drizzleLength` | Multiplies final Drizzle duration. | **Next duration calculation**; assume the next season transition for a running schedule. |
| `clearanceleLength` | Multiplies final Clearance duration. The spelling is from the game API. | Same as above. |
| `stormLength` | Multiplies final Storm duration. | Same as above. |
| `resolveToReputationRatio` | Multiplies resolve-to-reputation conversion. | **Next resolve reputation calculation.** |
| `leavingRate` | Multiplies the villager leaving rate. | **Next leaving check/calculation.** |
| `newcommersGoodsRate` | Multiplies goods generated for newcomer groups. | **Next newcomer-goods generation.** Existing waiting groups are unchanged. |
| `traderGlobalSellPriceRate` | Multiplies trader sell values and the displayed global price ratio. | **Next trade-price calculation/read.** |
| `tradeRoutesSpeed` | Multiplies trade-route speed. | **Next route speed or travel-time calculation.** |
| `villagersBreakTimeRate` | Multiplies villager break duration; higher values mean longer breaks. | **Next break-time calculation/new break.** |
| `globalSpeedFactor` | Multiplies global actor movement speed. | **Next actor-speed read.** |
| `roadsSpeedFactor` | Additionally multiplies on-road actor speed. | **Next on-road actor-speed read.** |
| `offroadSpeedFacotr` | Additionally multiplies off-road actor speed. The spelling is from the game API. | **Next off-road actor-speed read.** |
| `resolveNegativeChangeRate` | Multiplies negative Resolve change per second. | **Next Resolve-decay calculation.** |
| `tradersIntervalRate` | Multiplies trader arrival/travel interval speed. | **Next trader scheduling or time-left calculation.** |
| `newcomersIntervalRate` | Multiplies newcomer arrival interval speed. | **Next newcomer scheduling/progress calculation.** |
| `enginesBlightRate` | Multiplies Rainpunk engine blight/cyst efficiency. | **Next engine water-per-cyst or blight-rate calculation.** |
| `newYearEffectMultiplayer` | Multiplies the number of New Year effect iterations. The field is changed only for the original method call and restored by a finalizer. | **Next New Year event only.** No effect merely by opening or closing the panel. |
| `hungerMultiplier` | Multiplies hunger applications; `0` suppresses them. | **Next hunger application/update.** |

Setting a season-duration multiplier to `0` can request a zero-length season and should be treated as a degenerate, potentially disruptive value.

## Standalone gameplay controls

| Setting | Effect | When it takes effect |
|---|---|---|
| `Farm / Square area radius` (`0–10`) | Overrides both farm-area axes with the same margin. `0` preserves the model work area plus MetaPerks. Updates visual bounds, usable-field cache, and placement overlay. | **Immediately recalculates active farms when changed.** New placements use the current value; rebuilding the farm is not required. An already visible overlay may need to be redrawn. |
| `House / Extra places per house` (`0–20`) | Adds real capacity to every house while keeping total places, remaining places, move-in checks, and previews consistent. | **Next capacity/UI/move-in read.** Existing houses do not need to be rebuilt. |
| `Newcomers / Extra villagers per group` (`0–20`) | Adds villagers after the original random base, year scaling, and existing bonuses. Race assignment and goods setup still run normally. | **Next newly generated group.** Already waiting groups are unchanged. |
| `Hostility / Ignore negative effect hostility` | Skips both application and removal of `HostilitySource.EffectsNegative`. Existing points from that source are cleared and total Hostility is synchronized. Other sources and positive effects continue normally. | **Live for future add/remove operations.** Existing points are cleared **when enabled** and **when the Hostility monitor is set up**, including game load/new game. |
| `Season Traits / Enable per-season counts` (off by default), `Drizzle positive`, and `Storm negative` (`0–5`, default `1`) | Replaces only the Drizzle Positive and Storm Negative baseline categories with exact seeded selections. Counts are not clamped to baseline totals, so Drizzle Positive can increase up to five when candidates exist. Other regular effects and Static traits are preserved. | **New settlement seasonal-effect generation only.** Loaded saves skip regeneration, so changing this mid-settlement has no effect on that save. |
| `Embark / Goods amount multiplier` | See the world map/embark table above. | **Next embark-goods generation**, not on reload within an existing settlement. |

## Important interaction and persistence notes

- Resolve reputation can be scaled by both `ResolveMultiplier` and `ReputationMutiplier`; the effective result can therefore compound.
- CheatConfig season multipliers and EffectsState season multipliers are separate layers and multiply together.
- EffectsState read-time multipliers do not directly modify saved EffectsState values, avoiding repeated multiplication on every reload.
- `InfiniteCornerstoneRerolls` and `MoarSeasonRewards` are exceptions to the general no-direct-state-write pattern: rerolls write current save state, and MoarSeasonRewards changes persistent meta perks.
- Enabling a setting does not necessarily undo existing generated content. For example, it does not replace current blueprint picks, reroll current seasonal traits, change an existing caravan, or rewrite waiting newcomer groups.
- Disabling one-shot unlocks does not necessarily revoke them. In particular, building-move enablement is not removed by turning its setting off.

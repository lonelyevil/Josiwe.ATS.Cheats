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
- Presets do **not** include or reset the standalone **EffectsState Multipliers**, **Season Traits**, **Farm**, **House**, **Newcomers**, **Hostility**, **Embark**, or **Trader** settings. Those values remain unchanged when a preset is applied.
- Applying a preset does not reroll current seasonal traits, regenerate existing rewards, rebuild farms, or rewrite already generated caravans.

## World map, camera, and embark settings

| Setting | Effect | When it takes effect |
|---|---|---|
| `ZoomMultiplier` | Higher values increase maximum zoom-out and zoom speed; lower values reduce them. There is no enforced range. | **Next `CameraController.SetUp`**, normally a map/scene load, reload, or new settlement. Not live for an already initialized camera. |
| `AllRacesInWorldMap` | Replaces vanilla revealed-race generation with the mod's random 1–4 generation path. | **Next caravan generation** on the world map. Existing caravans are unchanged. |
| `BonusPreparationPoints` (`0–100`) | Higher values grant more extra preparation points; lower values grant fewer. `0` preserves the original bonus. | **Next embark/preparation-point calculation**, normally when the embark screen reads it. |
| `Embark / Goods amount multiplier` (`1–50`) | Higher values produce more caravan and embark-bonus goods after MetaPerks; lower values produce fewer. `1` is vanilla. | **Next embark-goods generation.** Existing settlement inventories are unchanged. |

## Reputation, impatience, blueprints, and cornerstones

| Setting | Effect | When it takes effect |
|---|---|---|
| `WildcardBlueprints` | Replaces the initial building picks with wildcard picks and replaces future reputation-threshold blueprint rewards with wildcard picks. | Initial replacement is **new settlement only**. Future threshold rewards change at the **next reputation reward generation**. Existing picks are not replaced. |
| `BlueprintsMultiplier` | Higher values grant more initial wildcard picks; lower values grant fewer. `0` grants none when `WildcardBlueprints` is enabled. | **New settlement only**, when initial reputation picks are prepared. It is not the count for every later threshold reward. |
| `InfiniteCornerstoneRerolls` | Sets the current cornerstone reroll count to 99 whenever the game reads it. | **Next reroll-count read**, effectively live for UI and reroll checks. This writes `CornerstonesState.rerollsLeft`, so it can enter save data. |
| `ResolveMultiplier` | Higher values increase Resolve-derived reputation; values between `0` and `1` reduce it; `0` gives none. Negative values preserve the original Resolve calculation. | **Next resolve reputation gain.** Resolve gains are also processed by `ReputationMutiplier`, so the two settings can compound. |
| `ReputationMutiplier` | Higher values increase each reputation addition; values between `0` and `1` reduce it; `0` prevents added reputation. Negative values preserve vanilla only when no stopgap is active. | **Next reputation addition.** |
| `ImpatienceMultiplier` | Higher values increase impatience gain; values between `0` and `1` reduce it; `0` prevents additions. Negative values preserve vanilla only when no stopgap is active. | **Next impatience addition.** |
| `ReputationStopgap` | Higher values stop Resolve reputation farther below the win threshold; lower values leave a smaller gap. `0` applies no extra stopgap. | **Next reputation calculation/addition.** |
| `ImpatienceStopgap` | Higher values stop impatience farther below the loss threshold; lower values leave a smaller gap. `0` applies no extra stopgap. | **Next impatience calculation/addition.** |
| `MoarMaxReputation` | Positive values require more reputation to win, making victory slower; negative values require less. `0` preserves the original threshold. | **Next win-threshold read**, such as UI or win checks. |
| `MoarMaxImpatience` | Positive values allow more impatience before losing; negative values allow less and make losing easier. `0` preserves the original threshold. | **Next loss-threshold read.** |
| `MoarSeasonRewards` | Raises cornerstone option generation toward four options and removes the normal exclusion list, allowing repeats. | **Next cornerstone reward generation.** It also increases persistent meta `bonusSeasonRewardsAmount`; disabling the setting does not revert that profile-side effect. |
| `CornerstonePicksPerSeason` | Higher values create more separate cornerstone picks per season; lower values create fewer. Values `1` or below preserve vanilla generation. | **Next seasonal cornerstone-pick generation.** Existing waiting picks remain. |
| `CashRewardMultiplier` | Higher values give more goods for declining a cornerstone; lower values give fewer. `0` gives nothing; negative values preserve vanilla. | **Next cornerstone decline.** |
| `EnableAllBuildingsMoving` | Enables moving most buildings through the game controller hook. | **Next `GameController.StartGame`**, normally a new game or reload. Enabling mid-game is not retroactive. Disabling does not revoke an already enabled mover state. |

## Season length settings

| Setting | Effect | When it takes effect |
|---|---|---|
| `DrizzleLengthMultiplier` | Values above `1` make Drizzle longer; values between `0` and `1` make it shorter. `0` or below preserves the original duration rate. | **Next season-duration calculation.** Assume the current season keeps its established end time; the next transition is the safe expectation. |
| `ClearanceLengthMultiplier` | Values above `1` make Clearance longer; values between `0` and `1` make it shorter. `0` or below preserves the original duration rate. | Same as above. |
| `StormLengthMultiplier` | Values above `1` make Storm longer; values between `0` and `1` make it shorter. `0` or below preserves the original duration rate. | Same as above. |

The separate EffectsState season controls below are applied at a different layer. They multiply the final duration after these CheatConfig season multipliers, rather than replacing them.

## Prestige / ascension settings

These settings modify ascension effect models while initial effects are being built for embark. They are **next embark / new settlement** settings. They do not retroactively rewrite an active settlement, and loading an existing save does not rerun the generation path. Custom-game and tutorial branches can intentionally bypass parts of this logic.

The table below describes the direction when the mod replaces the modifier amount. Guard values matter: negative values leave the original Prestige effect unchanged for Prestige 2, 4, 5, 6, 7, 8, 13, 14, 15, and 17. Positive values leave the original effect unchanged for Prestige 9, 10, 12, and 18. In those guard branches, the game keeps its original modifier instead of applying a stronger or weaker custom amount.

| Setting | Effect of increasing or decreasing the custom value |
|---|---|
| `Prestige_2_Amount` | Higher nonnegative values increase the Storm-length penalty; lower values reduce it. `0` neutralizes the modifier. |
| `Prestige_4_Amount` | Higher nonnegative values add more Amber to blueprint rerolls; lower values add less. `0` removes the extra cost. |
| `Prestige_5_Amount` | Higher nonnegative values make villagers leave faster; lower values make them leave more slowly. `0` neutralizes the modifier. |
| `Prestige_6_Amount` | Higher nonnegative values require more construction materials; lower values require fewer. `0` neutralizes the modifier. |
| `Prestige_7_Amount` | Higher nonnegative values increase the chance of extra food consumption; lower values reduce it. `0` removes the extra chance. |
| `Prestige_8_Amount` | Higher nonnegative values increase the chance of extra luxury consumption; lower values reduce it. `0` removes the extra chance. |
| `Prestige_9_Amount` | More negative values make relic work slower and strengthen the penalty; values closer to `0` weaken it. `0` neutralizes the modifier. |
| `Prestige_10_Amount` | More negative values lower trader sell prices further; values closer to `0` reduce the penalty. `0` neutralizes the modifier. |
| `Prestige_12_Amount` | More negative values provide fewer blueprint options; values closer to `0` provide more. `0` neutralizes the modifier. This is handled in the early pre-game pass. |
| `Prestige_13_Amount` | Higher nonnegative values directly add more Cornerstone options, reducing or reversing the penalty. `0` adds no options; negative values keep the original Prestige penalty. |
| `Prestige_14_Amount` | Higher nonnegative values reduce impatience reduction per reputation and strengthen the penalty; lower values weaken it. `0` neutralizes the modifier. |
| `Prestige_15_Amount` | Higher nonnegative values increase the Resolve threshold penalty; lower values reduce it. `0` neutralizes the modifier. |
| `Prestige_17_Amount` | Higher nonnegative values add more hunger stacks; lower values add fewer. `0` leaves the base single-stack behavior. |
| `Prestige_18_Amount` | More negative values make hearth sacrifice consumption faster; values closer to `0` make it slower and weaken the penalty. `0` neutralizes the modifier. |

## EffectsState multipliers

All 26 controls are independent range entries from `0` to `1000`, with `1` as neutral. Except for the temporary New Year guard described below, they multiply values at Harmony read points and do not write the configured factor into `EffectsState`.

“Live” below means the next relevant calculation, not necessarily an immediate redraw or reconstruction of an object that already cached the result.

| Setting | Effect | When it takes effect |
|---|---|---|
| `plantingSpeed` | Higher values plant faster; lower values plant more slowly. `0` stops planting progress. | **Next planting-rate read**, normally a planting action/update. |
| `harvestingSpeed` | Higher values harvest faster; lower values harvest more slowly. `0` stops harvesting progress. | **Next harvesting-rate read.** |
| `fuelConsumptionSpeed` | Higher values burn fuel faster and shorten burning time; lower values do the opposite. `0` produces an infinite/invalid burning-time calculation rather than a neutral result. | **Next fuel/burning-time calculation.** |
| `hearthSacraficeTimeRate` | Higher values slow sacrifice consumption; lower positive values accelerate it. `0` is degenerate because the original formula divides by this rate. | **Next hearth-sacrifice rate read.** |
| `constructionSpeed` | Higher values build faster; lower values build more slowly. `0` stops construction progress. | **Next construction-progress update.** |
| `constructionCost` | Higher values require more materials; lower values require fewer. `0` makes construction free. | **Next cost calculation/placement.** Already placed construction may retain previously calculated costs. |
| `relicsWorkingTimeRate` | Higher values finish relic work faster; lower values make it slower. The original min/max clamp still applies. | **Next relic working-time calculation.** |
| `grassAmountRatio` | Higher values generate more grass; lower values generate less. `0` generates none. | **Next grass calculation**, commonly map/glade generation rather than continuous terrain regeneration. |
| `drizzleLength` | Higher values make Drizzle longer; lower values make it shorter. `0` requests a zero-length season. | **Next duration calculation**; assume the next season transition for a running schedule. |
| `clearanceleLength` | Higher values make Clearance longer; lower values make it shorter. `0` requests a zero-length season. The spelling is from the game API. | Same as above. |
| `stormLength` | Higher values make Storm longer; lower values make it shorter. `0` requests a zero-length season. | Same as above. |
| `resolveToReputationRatio` | Higher values produce more reputation from Resolve; lower values produce less. `0` produces none. | **Next resolve reputation calculation.** |
| `leavingRate` | Higher values make villagers leave faster; lower values make them leave more slowly. `0` prevents Resolve-based leaving. | **Next leaving check/calculation.** |
| `newcommersGoodsRate` | Higher values generate more newcomer goods; lower values generate fewer. `0` generates none. | **Next newcomer-goods generation.** Existing waiting groups are unchanged. |
| `traderGlobalSellPriceRate` | Higher values increase sale value and currency received; lower values reduce it. `0` makes goods worthless to the trader. | **Next trade-price calculation/read.** |
| `tradeRoutesSpeed` | Higher values complete routes faster; lower values slow them. `0` prevents route completion. | **Next route speed or travel-time calculation.** |
| `villagersBreakTimeRate` | Higher values lengthen breaks and slow the consumption cycle; lower values shorten them. `0` requests zero-length breaks. | **Next break-time calculation/new break.** |
| `globalSpeedFactor` | Higher values increase global movement speed; lower values reduce it. `0` can stop movement. | **Next actor-speed read.** |
| `roadsSpeedFactor` | Higher values increase on-road speed; lower values reduce it. `0` stops on-road movement when the other factor is nonzero. | **Next on-road actor-speed read.** |
| `offroadSpeedFacotr` | Higher values increase off-road speed; lower values reduce it. `0` stops off-road movement when the other factor is nonzero. The spelling is from the game API. | **Next off-road actor-speed read.** |
| `resolveNegativeChangeRate` | Higher values increase negative Resolve change; lower values reduce it. `0` prevents Resolve decay from this calculation. | **Next Resolve-decay calculation.** |
| `tradersIntervalRate` | Higher values make traders arrive sooner; lower values delay them. `0` can prevent trader arrival/progress. | **Next trader scheduling or time-left calculation.** |
| `newcomersIntervalRate` | Higher values make newcomers arrive more frequently; lower values make them less frequent. `0` stops arrival progress. | **Next newcomer scheduling/progress calculation.** |
| `enginesBlightRate` | Higher values require less water per cyst and accelerate blight production; lower values require more. `0` effectively prevents cyst production. | **Next engine water-per-cyst or blight-rate calculation.** |
| `newYearEffectMultiplayer` | Higher integers run more New Year effect iterations; lower integers run fewer. `0` suppresses the New Year effect. The field is restored after the original call. | **Next New Year event only.** No effect merely by opening or closing the panel. |
| `hungerMultiplier` | Higher integers apply more hunger stacks; lower integers apply fewer. `0` suppresses hunger application. | **Next hunger application/update.** |

Setting a season-duration multiplier to `0` can request a zero-length season and should be treated as a degenerate, potentially disruptive value. More generally, `0` is not a universal “disable safely” value: it can also create free construction, stop movement or arrivals, or produce division-by-zero behavior in fuel and hearth formulas, as noted in each row.

## Standalone gameplay controls

| Setting | Effect | When it takes effect |
|---|---|---|
| `Farm / Square area radius` (`0–10`) | Higher values create a larger square work area; lower values create a smaller one. `0` preserves the model work area plus MetaPerks. Updates visual bounds, usable-field cache, and placement overlay. | **Immediately recalculates active farms when changed.** New placements use the current value; rebuilding the farm is not required. An already visible overlay may need to be redrawn. |
| `House / Extra places per house` (`0–20`) | Higher values add more real housing capacity; lower values add less. `0` preserves vanilla capacity. Total places, remaining places, move-in checks, and previews stay consistent. | **Next capacity/UI/move-in read.** Existing houses do not need to be rebuilt. |
| `Newcomers / Extra villagers per group` (`0–20`) | Higher values add more villagers after the original random base, year scaling, and bonuses; lower values add fewer. `0` preserves vanilla amounts. | **Next newly generated group.** Already waiting groups are unchanged. |
| `Hostility / Ignore negative effect hostility` | Skips both application and removal of `HostilitySource.EffectsNegative`. Existing points from that source are cleared and total Hostility is synchronized. Other sources and positive effects continue normally. | **Live for future add/remove operations.** Existing points are cleared **when enabled** and **when the Hostility monitor is set up**, including game load/new game. |
| `Season Traits / Enable per-season counts` (off by default), `Drizzle positive`, and `Storm negative` (`0–5`, default `1`) | Higher Drizzle Positive values create more positive Drizzle traits and lower values create fewer, with `0` creating none. Higher Storm Negative values create more negative Storm traits and lower values create fewer, with `0` creating none. Selection replaces only those categories; other regular and Static traits are preserved. | **New settlement seasonal-effect generation only.** Loaded saves skip regeneration, so changing this mid-settlement has no effect on that save. |
| `Embark / Goods amount multiplier` | See the world map/embark table above. | **Next embark-goods generation**, not on reload within an existing settlement. |
| `Trader / Manual refresh no impatience` | Prevents manual forced trader arrival from adding the Queen's impatience and stops repeated use from escalating its price. The usage counter is retained for statistics, while the displayed price remains the base value plus any current bonus. | **Next manual forced-arrival payment or price read.** No global suppression of impatience from other sources. |
| `Trader / Minimum perk count` (`1–6`, default `1`) | Tops up the selected special-effect/Perk list to the requested minimum without trimming a larger original selection. | **Next trader visit creation.** Existing visits and their saved offers are unchanged. |
| `Trader / Goods amount multiplier` (`1–10`, default `1`) | Multiplies the quantity of every random and guaranteed Good, rounding to an integer, without changing the number or types of goods. Higher values carry more; lower values carry less. | **Next trader visit creation.** Existing visits are unchanged. |
| `Trader / Guarantee wildcard perk` (off by default) | Ensures one eligible `WildcardPickEffectModel` is selected when available, replacing a non-wildcard entry rather than increasing the list count. The minimum Perk top-up runs afterward. | **Next trader visit creation.** If no eligible wildcard exists, the game keeps a valid selection and logs the failure. |

## Important interaction and persistence notes

- Resolve reputation can be scaled by both `ResolveMultiplier` and `ReputationMutiplier`; the effective result can therefore compound.
- CheatConfig season multipliers and EffectsState season multipliers are separate layers and multiply together.
- EffectsState read-time multipliers do not directly modify saved EffectsState values, avoiding repeated multiplication on every reload.
- `InfiniteCornerstoneRerolls` and `MoarSeasonRewards` are exceptions to the general no-direct-state-write pattern: rerolls write current save state, and MoarSeasonRewards changes persistent meta perks.
- Enabling a setting does not necessarily undo existing generated content. For example, it does not replace current blueprint picks, reroll current seasonal traits, change an existing caravan, or rewrite waiting newcomer groups.
- Disabling one-shot unlocks does not necessarily revoke them. In particular, building-move enablement is not removed by turning its setting off.

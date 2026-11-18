# Configuration Parameter Overlap Analysis

The project does not bind two different configuration keys to the same entry, but several settings affect the same final gameplay value. Their main differences are the application layer, scope, and activation timing.

See [Gameplay Effects and Activation Timing](EFFECTS_AND_TIMING.md) for the detailed timing of each setting.

## Highly overlapping parameters

| Parameter group | Overlap | Main distinction |
|---|---|---|
| `DrizzleLengthMultiplier` ↔ `drizzleLength` | Both change the final Drizzle duration and can multiply together. | The first belongs to CheatConfig and presets; the second is an EffectsState read-time multiplier. |
| `ClearanceLengthMultiplier` ↔ `clearanceleLength` | Both change the final Clearance duration. | Same distinction as above. |
| `StormLengthMultiplier` ↔ `stormLength` | Both change the final Storm duration. | Same distinction as above. |
| `Prestige_2_Amount` ↔ the Storm controls above | All three can affect Storm length, so three layers can apply simultaneously. | Prestige 2 changes the Ascension effect at the next embark; the other controls are general season-duration multipliers. |
| `ResolveMultiplier` ↔ `ReputationMutiplier` ↔ `resolveToReputationRatio` | All three can amplify reputation produced by Resolve, potentially creating three stacked multipliers. | `ReputationMutiplier` affects every reputation source; the other two affect different stages of the Resolve path. |
| `Prestige_5_Amount` ↔ `leavingRate` | Both change the villager leaving rate. | Prestige changes its Ascension effect for a new settlement; EffectsState applies a general read-time multiplier. |
| `Prestige_6_Amount` ↔ `constructionCost` | Both change construction cost. | One changes a Prestige-specific effect; the other multiplies the final cost read. |
| `Prestige_9_Amount` ↔ `relicsWorkingTimeRate` | Both change relic/glade-event working speed. | Their timing and scope differ, but the final effects can stack. |
| `Prestige_10_Amount` ↔ `traderGlobalSellPriceRate` | Both change trader sell prices. | Prestige writes its effect during embark; EffectsState applies when trade prices are read. |
| `Prestige_17_Amount` ↔ `hungerMultiplier` | Both change hunger stacks applied when villagers do not eat. | One modifies a Prestige effect model; the other multiplies the `GetHungerMultiplier` result. |
| `Prestige_18_Amount` ↔ `hearthSacraficeTimeRate` | Both change hearth sacrifice consumption speed. | This is another distinction between a Prestige-only effect and a general read-time multiplier. |

The three season-length pairs are the closest cases to direct duplication. If there is no need to keep both preset-oriented multipliers and uniform EffectsState multipliers, one control per season could be retained instead.

## Partial overlap with independent purposes

| Parameter group | Relationship and distinction |
|---|---|
| `MoarMaxReputation` ↔ `ReputationStopgap` | For Resolve reputation, the effective cap is approximately `base cap + MoarMaxReputation − ReputationStopgap`. However, the first also changes the overall win bar, while the stopgap restricts only Resolve gains; orders and map events can still win. |
| `MoarMaxImpatience` ↔ `ImpatienceStopgap` | The effective cap is approximately `base cap + MoarMaxImpatience − ImpatienceStopgap`. The first changes the overall loss threshold, while the stopgap clamps the result when impatience is added. |
| `MoarSeasonRewards` ↔ `Prestige_13_Amount` | Both can affect the number of Cornerstone options. The former pushes generation toward four options and removes the exclusion list; the latter changes the Prestige option-reduction effect. |
| `WildcardBlueprints` ↔ `BlueprintsMultiplier` ↔ `Prestige_12_Amount` | These are related but not duplicates: they respectively control wildcard replacement, the initial wildcard-pick count, and normal blueprint reward options. |
| `Newcomers / Extra villagers per group` ↔ `newcommersGoodsRate` | Extra villagers can indirectly affect goods distribution, but one primarily controls population while the other directly scales goods. |
| `globalSpeedFactor` ↔ `roadsSpeedFactor` / `offroadSpeedFacotr` | This is an intentional global-plus-conditional design. It allows global movement changes or independent road/off-road changes and should not be merged. |
| `Season Traits / Storm negative` ↔ `Hostility / Ignore negative effect hostility` | One controls how many Storm Negative traits are generated; the other suppresses Hostility produced by negative effects without removing the traits. |

## Features without significant duplication

The following controls currently have distinct purposes:

- `Farm / Square area radius`
- `House / Extra places per house`
- `Newcomers / Extra villagers per group`
- `Embark / Goods amount multiplier`
- `Trader / Manual refresh no impatience`
- `Hostility / Ignore negative effect hostility`
- The Drizzle Positive and Storm Negative allocations in `Season Traits`
- `InfiniteCornerstoneRerolls`
- `EnableAllBuildingsMoving`

Patching both `GetHouseCapacity(House)` and `GetHouseCapacity(HouseModel)` is intentional so actual capacity and model previews stay consistent. Likewise, patching both trader price reads keeps actual prices and the displayed ratio consistent. These are not duplicate settings.

## Simplification recommendations

If the goal is to make the F1 panel easier to understand, consider these steps in order:

1. **Keep only one general multiplier per season duration**, or explicitly label the two layers as preset-oriented and final-duration multipliers.
2. **Retain one Resolve-specific reputation multiplier** in addition to the global `ReputationMutiplier`; the current three-control path can produce unexpected stacking.
3. **Label Prestige parameters as Next Embark / Ascension Only** and keep them visually separate from live global multipliers.
4. **Keep the Max and Stopgap controls only if both behaviors are needed**, and document their effective-cap formulas in the panel descriptions.
5. **Do not merge the complementary groups** for wildcard blueprints, Cornerstone picks versus options, newcomer population/goods/intervals, or global versus conditional movement speed.

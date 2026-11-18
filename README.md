# Against The Storm Cheats

The compiled mod is meant to be loaded into the game with [BepInEx](https://github.com/BepInEx/BepInEx). 
I tested with BepInEx_x64_5.4.22.0 on Windows 11 (https://github.com/BepInEx/BepInEx/releases/download/v5.4.22/BepInEx_x64_5.4.22.0.zip). 
Simply download the loader and unzip it into your game directory. 
For the Steam version of the game, this will likely be `C:\Program Files (x86)\Steam\steamapps\common\Against the Storm\`. 

The `.dll` file created by this mod (`Josiwe.ATS.Cheats.dll` by default) can then be dropped into the `BepInEx\plugins` subdirectory 
inside the game directory. You might have to make this directory yourself, or run the game once to have it made automatically.

You can use one of the `.json` files which are preset changes. Put these in the same directory and rename them to `Josiwe.ATS.Cheats.Config.json`.
If you do not have one, the mod uses your BepInEx/Thunderstore configuration instead.

We've also added example configs to fit a few playstyles! Feel free to replace single lines, or whole sections in the main config file:

- If you prefer the vanilla game, there's now a `Vanilla.Config.json` file for you
- If you like exploring, for instance, there's an `Explorer.Config.json` you can use as a reference
- If you're one of those people looking for a _real_ challenge, take a crack at the `GluttonForPunishment.Config.json` =)

Feel free to mix and match them all, there's a million ways to have a blast with this game.

# ConfigurationManager panel

With [BepInEx.ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) installed, press **F1** after the game starts to edit every cheat setting. Changes update the values used by the mod and persist through BepInEx; when `Josiwe.ATS.Cheats.Config.json` is present, panel changes are also written back to that file.

The **Presets** section contains an **Apply preset** dropdown with **Explorer**, **Vanilla**, and **Glutton for Punishment**. Selecting one applies the full preset immediately and resets the dropdown so it can be used again. Settings that only affect embarking, game start, or other one-time game hooks still take effect at their usual time.

The **EffectsState Multipliers** section adds an independent slider and text box for each of the 26 lower-level numeric multipliers. These range from **0 to 10** and default to **1**, which preserves the game's current value. They are applied when the game reads the value, so F1 changes take effect without directly modifying the saved `EffectsState`. Setting the integer New Year or Hunger multiplier to `0` suppresses its effect.

The **Farm** section contains **Square area radius** (`0–10`). The default `0` preserves the farm model's work area and MetaPerks bonus. Values `1–10` replace both axes with the same margin, updating the placement overlay, visible area, and actual usable-field cache together. Total dimensions are the farm building size plus twice the radius; active farms are recalculated when the setting changes.

The **House** section contains **Extra places per house** (`0–20`). The default `0` preserves vanilla capacity. Positive values add real capacity to every house, consistently updating total places, remaining places, move-in checks, HUD values, and building previews.

The **Newcomers** section contains **Extra villagers per group** (`0–20`). The default `0` preserves vanilla amounts. Positive values are added after the original random base, year scaling, and existing newcomer bonuses; the extra villagers then use the normal race assignment and goods setup. This applies to newly generated groups and does not rewrite groups already waiting.

The **Season Traits** section contains **Enable per-season counts** and independent **Drizzle positive**, **Clearance positive**, and **Storm positive** values (`0–10`, default `1`). While enabled, the game first generates its normal baseline, then keeps each season's regular-trait total and calculates `negative = baseline total − configured positive` (positive is clamped to that total). Static traits are preserved separately and do not count toward this allocation. Actual counts remain limited by eligible unique candidates. Existing saves are not regenerated.

The **Hostility** section contains **Ignore negative effect hostility** (default off). When enabled, `HostilitySource.EffectsNegative` is completely ignored: adding or removing negative effects cannot change Hostility. Existing points from this source are cleared on enable and when the Hostility monitor is set up; other Hostility sources and positive effects continue normally.

# Development setup

Copy `Local.props.example` to `Local.props`, then edit `Local.props` in Visual Studio and set `StormPath` to your Against the Storm install and `BepInExPath` to your target BepInEx directory. The project imports this file for both Visual Studio and `dotnet build`; `Local.props` is ignored by Git so machine-specific paths stay local.

# Cheat Settings Data

Here you'll find a detailed description of the type and effect of each setting in the config files, split by sections.

## World Map:

**ZoomLimitMultiplier**
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- allows both the playable map and the world map to be zoomed out by a factor of X (the value you define)
- great at 3, will work nicely with most machines
- awesome at 7 or higher, but can slow things down to a crawl on low end machines

**AllRacesInWorldMap**
- Boolean: _true or false_
- when set to true it should allow a more random choice of races when picking a cell in the world map
- when set to false uses the game's default logic
- _experimental feature_

**BonusPreparationPoints**
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- setting this to any number greater than 0 will give you more embarkation points in the world map
- setting this to 0 uses the game's default logic

## Game Map:

**WildcardBlueprints**
- Boolean: _true or false_
- when set to true it replaces the game's random blueprint logic with wildcard picks; can be used in conjunction with the _BlueprintsMultiplier_ setting
- when set to false uses the game's default logic

**InfiniteCornerstoneRerolls**
- Boolean: _true or false_
- when set to true it allows the player to reroll cornerstones an infinite amount of times (also updates the UI to always show 99 rerolls left)
- when set to false uses the game's default logic

**ResolveMultiplier**
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- used as a multiplier for resolve gains (blue bars for each race at the top left of the map game UI)
- setting this to any number between 0.0 and 1.0 will make the peon's resolve bar fill at a slower rate
- setting this to any number higher than 1.0 will make peon's resolve bar fill at a faster rate
- setting this to 1.0 uses the game's default logic

**ReputationMutiplier**
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- used as a multiplier for reputation growth (blue bar at the bottom centre of the map game UI)
- setting this to 0.0 will make it so that _**only**_ loyalty map events and uber happy peons can increase your reputation
- setting this to any number between 0.0 and 1.0 will make the reputation bar fill at a slower rate
- setting this to any number higher than 1.0 will make the reputation bar fill at a faster rate
- setting this to 1.0 uses the game's default logic

**ImpatienceMultiplier**
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- used as a multiplier for impatience growth (red bar at the bottom centre of the map game UI)
- setting this to 0.0 will make it so that _**only**_ impatience map events can increase your impatience
- setting this to any number between 0.0 and 1.0 will make the impatience bar fill at a slower rate
- setting this to any number higher than 1.0 will make the impatience bar fill at a faster rate
- setting this to 1.0 uses the game's default logic

**ReputationStopgap**
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- setting this to any positive value greater than zero will stop the reputation bar at X points from the max 
	- only loyalty map events may increase it
	- e.g. when playing on prestige 20 the max is 14, so if you set the stopgap at 2 the bar won't fill up past 12 points
- setting this to 0 will allow the game's default logic to end your game when the max value for the map is reached (win scenario)

**ImpatienceStopgap**
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- setting this to any positive value greater than zero will stop the impatience bar at X points from the max
	- only impatience map events may increase it
	- e.g. when playing on prestige 20 the max is 14, so if you set the stopgap at 2 the bar won't fill up past 12 points
- setting this to 0 will allow the game's default logic to end your game when the max value for the map is reached (lose scenario)

**MoarMaxReputation**
- Integer: _accepts any signed whole number (i.e. -10, 30, 15, -20)_
- setting this value will grow (or shrink) the reputation bar on maps
- setting this to 1.0 uses the game's default logic

**MoarMaxImpatience**
- Integer: _accepts any signed whole number (i.e. -10, 30, 15, -20)_
- setting this value will grow (or shrink) the impatience bar on maps
- setting this to 1.0 uses the game's default logic

**BlueprintsMultiplier**
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- when set to any number greater than one, will allow a user to pick more blueprints
	- after either gaining a whole reputation point or buying the extra blueprint from a vendor
- setting this to 1 uses the game's default logic

**CashRewardMultiplier**
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- when set to any number greater than one, will allow a user to get more money when declining a cornerstone pick
- setting this to 1 uses the game's default logic

**CornerstonePicksPerSeason**
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- when set to any number greater than one, will allow a user to pick more cornerstones per season
- setting this to 1 uses the game's default logic

**EnableAllBuildingsMoving**
- Boolean: _accepts true or false_
- When set to true, most buildings can be moved allowing the user to tweak the layout of their empire.
- setting this to false uses the game's default logic
- uncertain how this will act if the world map feature which grants it is used at the same time
<br>

## Seasonal:

**MoarSeasonRewards**
- Boolean: _true or false_
- when set to true it maxes out the amount of items displayed for each cornerstone pick to 7 (the max the UI can display without major issues)
- when set to false uses the game's default logic

**StormLengthMultiplier**
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 1.0 will make storm seasons shorter
- setting this to any number higher than 1.0 will make storm seasons longer
- setting this to 1.0 uses the game's default logic

**DrizzleLengthMultiplier**
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 1.0 will make drizzle seasons shorter
- setting this to any number higher than 1.0 will make drizzle seasons longer
- setting this to 1.0 uses the game's default logic

**ClearanceLengthMultiplier**
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 1.0 will make clearance seasons shorter
- setting this to any number higher than 1.0 will make clearance seasons longer
- setting this to 1.0 uses the game's default logic

## Difficulties: _(work in progress...)_

**Prestige_2_Amount**
<br>`Longer Storm - One of the seals is loosening its grip, leaking darkness upon this land. Storm season lasts 100% longer`
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 1.0 will the penalty a bit smaller
- setting this to any number higher than 1.0 will make the penalty larger
- setting this to 1.0 uses the game's default logic

**Prestige_4_Amount**
<br>`Higher Blueprints Reroll Cost - The Archivist assigned to your settlement is fiercely loyal to the Royal Court, so bribing him will be more expensive. Blueprint rerolls cost 10 Amber more` 
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- when set to any number greater than one, will make the penalty larger
- setting this to 10 uses the game's default logic

**Prestige_5_Amount**
<br>`Faster Leaving - Villagers are less understanding than they used to be. They're probably getting a bit spoiled by now. Villagers are 100% faster to leave if they have low Resolve`
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 1.0 will the penalty a bit smaller
- setting this to any number higher than 1.0 will make the penalty larger
- setting this to 1.0 uses the game's default logic

**Prestige_6_Amount**
<br>`Wet Soil - It's particularly hard to build anything in this region. Buildings require 50% more materials`
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 0.5 will the penalty a bit smaller
- setting this to any number higher than 0.5 will make the penalty larger
- setting this to 0.5 uses the game's default logic

**Prestige_7_Amount**
<br>`Parasites - One of the villagers was sick, and infected the rest of the settlement with a parasite. All villagers have a 50% chance of eating twice as much during their break`
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 0.5 will the penalty a bit smaller
- setting this to any number higher than 0.5 will make the penalty larger
- setting this to 0.5 uses the game's default logic

**Prestige_8_Amount**
<br>`Higher Needs Consumption Rate - Villagers have forgotten what a modest life looks like. They want to enjoy life to the fullest. Villagers have a 50% chance to consume double the amount of luxury goods`
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 0.5 will the penalty a bit smaller
- setting this to any number higher than 0.5 will make the penalty larger
- setting this to 0.5 uses the game's default logic

**Prestige_9_Amount**
<br>`Longer Relics Working Time - Villagers are reluctant to venture into Dangerous Glades. Scouts work 33% slower on Glade Events`
- Float: _accepts any **negative** floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number lower than -0.33 will the penalty larger
- setting this to any number between -0.33 and -0.0 will make the penalty smaller
- setting this to -0.33 uses the game's default logic

**Prestige_10_Amount**
<br>`Higher Traders Prices - Traders gossip about you doing pretty well lately. All your goods are worth 50% less to traders`
- Float: _accepts any **negative** floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number lower than -0.5 will the penalty larger
- setting this to any number between -0.5 and -0.0 will make the penalty smaller
- setting this to -0.5 uses the game's default logic

**Prestige_12_Amount**
<br>`Fewer Blueprints Options - The greedy Royal Archivist sold most of the blueprints to traders and fled the Citadel. You have 2 fewer blueprint choices`
- Integer: _accepts any **negative** whole number (i.e. -1, -3, -5)_
- when set to any negative number, will make the penalty larger
- setting this to -2 uses the game's default logic

**Prestige_13_Amount**
<br>`Fewer Cornerstones Options - The Royal Envoy comes to you with bad news. The Queen has restricted your cornerstone choices by 2`
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- when set to any number greater than one, will make the penalty larger
- setting this to 1 uses the game's default logic

**Prestige_14_Amount**
<br>`Lower Impatience Reduction - The Queen expects a lot from a viceroy of your rank. Impatience falls by 0.5 points less for every Reputation Point you gain`
- Float: _accepts any positive floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number between 0.0 and 0.5 will the penalty a bit smaller
- setting this to any number higher than 0.5 will make the penalty larger
- setting this to 0.5 uses the game's default logic

**Prestige_15_Amount**
<br>`Global Reputation Treshold Increase - You took a very peculiar group of settlers with you. They seem perpetually dissatisfied. The Resolve threshold at which each species starts producing Reputation increases by 1 more point for every Reputation Point they generate`
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- when set to any number greater than one, will make the penalty larger
- setting this to 1 uses the game's default logic

**Prestige_17_Amount**
<br>`Hunger Multiplier Effects - Famine outbreaks in your previous settlements have made the villagers particularly sensitive to food shortages. Every time villagers have nothing to eat during a break, they will gain two stacks of the Hunger effect instead of one`
- Integer: _accepts any positive whole number greater than zero (i.e. 1, 3, 5)_
- when set to any number greater than one, will make the penalty larger
- setting this to 1 uses the game's default logic

**Prestige_18_Amount**
<br>`Faster Fuel Sacrifice - The Ancient Hearth seems to have a defect. No matter how hard the firekeeper tries, sacrificed resources are burning 35% quicker`
- Float: _accepts any **negative** floating point number, usually only one or two decimals (i.e. 1.0, 0.3, 2.45)_
- setting this to any number lower than -0.35 will the penalty larger
- setting this to any number between -0.35 and -0.0 will make the penalty smaller
- setting this to -0.35 uses the game's default logic

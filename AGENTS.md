# AGENTS.md

## Project map

- Single `netstandard2.0` BepInEx 5 / Harmony mod for Against the Storm. `Plugin.cs` contains `Awake()` and all patches; `CheatConfig.cs` defines the JSON and BepInEx settings.
- `Awake()` loads configuration before applying every patch on `Plugin`. Harmony prefixes return `false` to skip the original game method and `true` to run it; a null configuration should leave vanilla behavior intact.
- The project publicizes the game's `Assembly-CSharp.dll`; accessing otherwise-private game members is intentional. Rebuild against the installed game after game updates.
- Prestige patches switch on `modifier.effect.DisplayNameKey` and cast to concrete effect models. Use the existing patch log output to identify the current game's keys and model types before adding or updating one.

## Build and verification

For Visual Studio, copy `Local.props.example` to the Git-ignored `Local.props` and set both paths there; the csproj imports it automatically. Never move real local paths into tracked project files.

For an isolated command-line build, run from the repository root in PowerShell and override both properties:

```powershell
dotnet build .\Josiwe.ATS.Cheats.csproj `
  "-p:StormPath=C:\path\to\Against the Storm" `
  "-p:BepInExPath=$env:TEMP\Josiwe.ATS.Cheats.Deploy"
```

- The game DLLs under `Against the Storm_Data\Managed` are mandatory compile references; there are no stubs.
- Every successful build runs `Deploy` and copies the output into `$(BepInExPath)\plugins\Joswie`. Keep `BepInExPath` pointed at a scratch directory for compile-only verification so the live Thunderstore profile is not modified.
- The three alternate preset JSONs are copied to build output and deployed beside the DLL. The main `Josiwe.ATS.Cheats.Config.json` is not auto-deployed; copy and rename a preset manually only when testing the JSON-first workflow.
- There are no test projects or CI workflows; `dotnet test` is not a meaningful check. Verify code changes by compiling, then testing with BepInEx 5.x in the game when behavior matters.
- `PluginInfo` is generated under `obj` from the csproj. Do not edit generated files; change `<Version>` in `Josiwe.ATS.Cheats.csproj`.

## Configuration compatibility

- Adding a setting requires a `CheatConfig` field, its `Init(...)` call in `ReadValues()`, the same key in all four root `*.Config.json` presets, and README documentation.
- Treat `CheatConfig.cs` as the source of truth for keys. In particular, the real key is `ZoomMultiplier` (README's `ZoomLimitMultiplier` is stale), and the shipped typo `ReputationMutiplier` must not be renamed without a compatibility migration.
- BepInEx entries are the live backing store for `CheatConfig` fields through `Config.SettingChanged`. Panel edits also update the main JSON when one was loaded; without JSON, the BepInEx `.cfg` is authoritative.
- The F1 preset selector is a separate `ConfigEntry<string>` with an `AcceptableValueList<string>`, not a `CheatConfig` field. It applies a whole preset and resets to its sentinel; preserve the reentrancy guard and startup reset.
- `EffectsStateMultipliers.cs` owns 26 independent range entries (`0..10`, neutral `1`). Apply them at game read points only; writing these factors into `EffectsState` would persist them and can compound them on each save/load. The New Year patch may change the field only inside its original method and must restore it in a finalizer.
- Patch `Farm.GetAreaSize()` for square farm-area overrides: it feeds `CalculateAreaRect`, `CacheFieldsInArea`, and `FarmPlanOverlay.WorkArea`. Radius `0` must preserve `model.workArea + MetaPerks`; positive values override both axes, and changes recalculate active farms.
- For extra housing, patch both `EffectsService.GetHouseCapacity(House)` and `GetHouseCapacity(HouseModel)` rather than only `House.GetPlacesLeft()`. This keeps real capacity, remaining places, move-in checks and previews consistent; bonus `0` is neutral.
- JSON heading and `*_Description` properties are inert documentation ignored by deserialization. Panel-driven JSON writes must preserve these unknown properties and avoid a BOM.
- Thunderstore has previously rejected a BOM in `CHANGELOG.md` (`7cb1e0f`); keep packaged text files UTF-8 without a BOM.

## Release identity

- Update the csproj `<Version>`, `manifest.json` `version_number`, and `CHANGELOG.md` together for a release.
- `JoswieCheats` in the manifest and `plugins/Joswie` in deployment are shipped names, not typos to clean up. Likewise, `GetSeasonLenghtRate` is the game API's spelling and is required by the Harmony patch.

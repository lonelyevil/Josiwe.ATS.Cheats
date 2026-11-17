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
- Deployment copies only build output, not the root preset JSON files. For in-game testing, copy a preset beside the deployed DLL and rename it to `Josiwe.ATS.Cheats.Config.json`.
- There are no test projects or CI workflows; `dotnet test` is not a meaningful check. Verify code changes by compiling, then testing with BepInEx 5.x in the game when behavior matters.
- `PluginInfo` is generated under `obj` from the csproj. Do not edit generated files; change `<Version>` in `Josiwe.ATS.Cheats.csproj`.

## Configuration compatibility

- Adding a setting requires a `CheatConfig` field, its `Init(...)` call in `ReadValues()`, the same key in all four root `*.Config.json` presets, and README documentation.
- Treat `CheatConfig.cs` as the source of truth for keys. In particular, the real key is `ZoomMultiplier` (README's `ZoomLimitMultiplier` is stale), and the shipped typo `ReputationMutiplier` must not be renamed without a compatibility migration.
- Despite the fallback log/comments, `m_LoadedFromJSON` currently starts as `true` and is never set to `false`; consequently the no-JSON path writes field values into BepInEx rather than reading existing BepInEx values. Preserve this behavior unless a fix is explicitly requested.
- JSON heading and `*_Description` properties are inert documentation ignored by deserialization, not missing bindings.
- Thunderstore has previously rejected a BOM in `CHANGELOG.md` (`7cb1e0f`); keep packaged text files UTF-8 without a BOM.

## Release identity

- Update the csproj `<Version>`, `manifest.json` `version_number`, and `CHANGELOG.md` together for a release.
- `JoswieCheats` in the manifest and `plugins/Joswie` in deployment are shipped names, not typos to clean up. Likewise, `GetSeasonLenghtRate` is the game API's spelling and is required by the Harmony patch.

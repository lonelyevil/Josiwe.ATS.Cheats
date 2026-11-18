# Against The Storm Cheats

A BepInEx 5 mod that adds configurable cheats, gameplay multipliers, presets, and quality-of-life options for Against the Storm.

## Requirements

- Against the Storm.
- [BepInEx 5](https://github.com/BepInEx/BepInEx). The mod was tested with **BepInEx x64 5.4.22.0** on Windows 11.
- Optional but recommended: [BepInEx.ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) for the in-game **F1** configuration window. Use the BepInEx 5 build.

## Installation

### Mod manager / Thunderstore

Install the mod and its BepInEx dependency through your mod manager. Install BepInEx.ConfigurationManager separately if you want the F1 panel.

### Manual installation

1. Install BepInEx 5 into the game directory. For a typical Steam installation, this is:
   `C:\Program Files (x86)\Steam\steamapps\common\Against the Storm\`.
2. Place `Josiwe.ATS.Cheats.dll` in:
   `BepInEx\plugins\Joswie\`.
3. Keep `Explorer.Config.json`, `Vanilla.Config.json`, and `GluttonForPunishment.Config.json` beside the DLL if you want to use the preset dropdown. Project builds copy these files automatically.

Run the game once after installation so BepInEx can finish creating its configuration files.

## Configuration

### In-game F1 panel

With BepInEx.ConfigurationManager installed, press **F1** after the game starts.

- Changes are stored in the mod's BepInEx `.cfg` file.
- Updating a configuration value does not necessarily regenerate existing game state. Some cheats are live, while others apply only at the next reward, embark, season, or settlement.
- See [Gameplay Effects and Activation Timing](EFFECTS_AND_TIMING.md) for every setting and its exact activation timing.

### Optional main JSON configuration

You can place `Josiwe.ATS.Cheats.Config.json` beside the DLL.

- When this file exists, it is loaded at plugin startup and copied into BepInEx.
- Later F1 changes are also written back to this JSON file.
- Without this file, the BepInEx/Thunderstore `.cfg` remains authoritative; no main JSON file is created automatically.

For compatibility, use `ZoomMultiplier` (not the obsolete documentation name `ZoomLimitMultiplier`). The shipped spelling `ReputationMutiplier` remains intentional and should be kept in existing JSON files.

### Presets

Three presets are provided:

- **Explorer**
- **Vanilla**
- **Glutton for Punishment**

With the preset JSON files beside the DLL, select **Presets → Apply preset** in the F1 panel. Selecting a preset applies it and resets the dropdown so the same preset can be selected again.

Presets cover the original `CheatConfig` settings. They do not reset the standalone EffectsState, Season Traits, Farm, House, Newcomers, Hostility, or Embark controls.

As a manual alternative, copy one preset beside the DLL and rename it to `Josiwe.ATS.Cheats.Config.json`.

## Documentation

- [Gameplay Effects and Activation Timing](EFFECTS_AND_TIMING.md) — complete effect catalog, persistence notes, and whether changes are live, event-driven, embark-only, or new-settlement-only.
- [Configuration Parameter Overlap Analysis](PARAMETER_OVERLAP.md) — overlapping controls, intentional complementary settings, and simplification recommendations.
- [Changelog](CHANGELOG.md) — release history.

## Build from source

Requirements:

- A .NET SDK capable of building `netstandard2.0` projects.
- A local Against the Storm installation. The game DLLs in `Against the Storm_Data\Managed` are required compile references.

### Visual Studio or normal dotnet build

1. Copy `Local.props.example` to `Local.props`.
2. In `Local.props`, set:
   - `StormPath` to the Against the Storm installation directory.
   - `BepInExPath` to the target BepInEx directory.
3. Build `Josiwe.ATS.Cheats.csproj` in Visual Studio, or run:

```powershell
dotnet build .\Josiwe.ATS.Cheats.csproj
```

`Local.props` is ignored by Git, so machine-specific paths stay local.

Every successful build runs the `Deploy` target and copies the DLL, build output, and three preset JSON files to:

```text
$(BepInExPath)\plugins\Joswie
```

The main `Josiwe.ATS.Cheats.Config.json` is not deployed automatically.

### Isolated command-line build

To avoid modifying a live BepInEx profile, override both paths and deploy to a scratch directory:

```powershell
dotnet build .\Josiwe.ATS.Cheats.csproj `
  "-p:StormPath=C:\path\to\Against the Storm" `
  "-p:BepInExPath=$env:TEMP\Josiwe.ATS.Cheats.Deploy"
```

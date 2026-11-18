using System.Collections.Generic;
using BepInEx.Configuration;
using Eremite.Model.State;

namespace Josiwe.ATS.Cheats
{
    // Read-time multipliers for EffectsState values. Nothing here is ever written into
    // EffectsState, so save data and existing perks/effects stay untouched.
    internal sealed class EffectsStateMultipliers
    {
        internal const string SectionName = "EffectsState Multipliers";

        private const float NeutralFloat = 1f;
        private const int NeutralInt = 1;

        private readonly Dictionary<string, ConfigEntry<float>> m_Floats = new Dictionary<string, ConfigEntry<float>>();
        private readonly Dictionary<string, ConfigEntry<int>> m_Ints = new Dictionary<string, ConfigEntry<int>>();

        internal EffectsStateMultipliers(ConfigFile config)
        {
            // Ranges default to 0..10 and ShowRangeAsPercent stays false, so
            // ConfigurationManager draws a slider plus a text field for every entry.
            AddFloat(config, nameof(EffectsState.plantingSpeed));
            AddFloat(config, nameof(EffectsState.harvestingSpeed));
            AddFloat(config, nameof(EffectsState.fuelConsumptionSpeed));
            AddFloat(config, nameof(EffectsState.hearthSacraficeTimeRate));
            AddFloat(config, nameof(EffectsState.constructionSpeed));
            AddFloat(config, nameof(EffectsState.constructionCost));
            AddFloat(config, nameof(EffectsState.relicsWorkingTimeRate));
            AddFloat(config, nameof(EffectsState.grassAmountRatio));
            AddFloat(config, nameof(EffectsState.drizzleLength));
            AddFloat(config, nameof(EffectsState.clearanceleLength));
            AddFloat(config, nameof(EffectsState.stormLength));
            AddFloat(config, nameof(EffectsState.resolveToReputationRatio));
            AddFloat(config, nameof(EffectsState.leavingRate));
            AddFloat(config, nameof(EffectsState.newcommersGoodsRate));
            AddFloat(config, nameof(EffectsState.traderGlobalSellPriceRate));
            AddFloat(config, nameof(EffectsState.tradeRoutesSpeed));
            AddFloat(config, nameof(EffectsState.villagersBreakTimeRate));
            AddFloat(config, nameof(EffectsState.globalSpeedFactor));
            AddFloat(config, nameof(EffectsState.roadsSpeedFactor));
            AddFloat(config, nameof(EffectsState.offroadSpeedFacotr));
            AddFloat(config, nameof(EffectsState.resolveNegativeChangeRate));
            AddFloat(config, nameof(EffectsState.tradersIntervalRate));
            AddFloat(config, nameof(EffectsState.newcomersIntervalRate));
            AddFloat(config, nameof(EffectsState.enginesBlightRate));

            AddInt(config, nameof(EffectsState.newYearEffectMultiplayer));
            AddInt(config, nameof(EffectsState.hungerMultiplier));
        }

        internal float GetFloat(string fieldName)
        {
            return m_Floats.TryGetValue(fieldName, out ConfigEntry<float> entry)
                       ? entry.Value
                       : NeutralFloat;
        }

        internal int GetInt(string fieldName)
        {
            return m_Ints.TryGetValue(fieldName, out ConfigEntry<int> entry)
                       ? entry.Value
                       : NeutralInt;
        }

        private void AddFloat(ConfigFile config, string fieldName)
        {
            m_Floats[fieldName] = config.Bind(
                SectionName,
                fieldName,
                NeutralFloat,
                new ConfigDescription(
                    $"Read-time multiplier for EffectsState.{fieldName}. 1 is neutral (vanilla behavior); 0 disables its contribution. Applied when the game reads the value, so save data and existing perks/effects are not modified.",
                    new AcceptableValueRange<float>(0f, 10f)));
        }

        private void AddInt(ConfigFile config, string fieldName)
        {
            m_Ints[fieldName] = config.Bind(
                SectionName,
                fieldName,
                NeutralInt,
                new ConfigDescription(
                    $"Read-time multiplier for EffectsState.{fieldName}. 1 is neutral (vanilla behavior); 0 disables its contribution. Applied when the game reads the value, so save data and existing perks/effects are not modified.",
                    new AcceptableValueRange<int>(0, 10)));
        }
    }
}

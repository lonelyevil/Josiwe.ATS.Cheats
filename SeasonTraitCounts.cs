using BepInEx.Configuration;

namespace Josiwe.ATS.Cheats
{
    // Configured counts for the two seasonal trait categories this mod controls:
    // Drizzle positive effects and Storm negative effects. Both categories are picked
    // fresh at settlement generation; every other season/category keeps its vanilla
    // effects. Counts are exact, capped only by available unique candidates.
    internal sealed class SeasonTraitCounts
    {
        internal const string SectionName = "Season Traits";

        private readonly ConfigEntry<bool> m_Enabled;
        private readonly ConfigEntry<int> m_DrizzlePositive;
        private readonly ConfigEntry<int> m_StormNegative;

        internal SeasonTraitCounts(ConfigFile config)
        {
            m_Enabled = config.Bind(
                SectionName,
                "Enable per-season counts",
                false,
                new ConfigDescription(
                    "Replaces the Drizzle positive and Storm negative seasonal effects with the configured counts when a settlement's seasonal effects are generated. Every other seasonal effect keeps the game's normal selection. Off keeps the game's normal seasonal effect generation.",
                    null));

            // Default 1 keeps a single effect per controllable category. A range and no
            // percent display makes ConfigurationManager draw a slider with a text box.
            m_DrizzlePositive = config.Bind(
                SectionName,
                "Drizzle positive",
                1,
                new ConfigDescription(
                    "Number of positive seasonal effects for Drizzle while per-season counts are enabled. Values above the number of eligible candidates simply yield all of them.",
                    new AcceptableValueRange<int>(0, 5)));

            m_StormNegative = config.Bind(
                SectionName,
                "Storm negative",
                1,
                new ConfigDescription(
                    "Number of negative seasonal effects for Storm while per-season counts are enabled. Values above the number of eligible candidates simply yield all of them.",
                    new AcceptableValueRange<int>(0, 5)));
        }

        internal bool Enabled
        {
            get { return m_Enabled.Value; }
        }

        internal int DrizzlePositive
        {
            get { return m_DrizzlePositive.Value; }
        }

        internal int StormNegative
        {
            get { return m_StormNegative.Value; }
        }
    }
}

using System.Collections.Generic;
using BepInEx.Configuration;
using Eremite.Model;

namespace Josiwe.ATS.Cheats
{
    // Configures only the positive seasonal trait count for each season. Negative traits are
    // derived from the season's vanilla total, so both pools always add up to that total.
    internal sealed class SeasonTraitCounts
    {
        internal const string SectionName = "Season Traits";

        private readonly ConfigEntry<bool> m_Enabled;
        private readonly Dictionary<Season, ConfigEntry<int>> m_Positives =
            new Dictionary<Season, ConfigEntry<int>>();

        internal SeasonTraitCounts(ConfigFile config)
        {
            m_Enabled = config.Bind(
                SectionName,
                "Enable per-season counts",
                false,
                new ConfigDescription(
                    "Replaces seasonal effect generation with the per-season positive counts below. Negative traits are the season's normal total minus this positive count. Off keeps the game's normal seasonal effect generation.",
                    null));

            // Default 1 positive per season; that season's remaining baseline traits stay
            // negative. A range and no percent display makes ConfigurationManager draw a
            // slider with a text box.
            AddPositive(config, Season.Drizzle, "Drizzle positive");
            AddPositive(config, Season.Clearance, "Clearance positive");
            AddPositive(config, Season.Storm, "Storm positive");
        }

        internal bool Enabled
        {
            get { return m_Enabled.Value; }
        }

        internal int GetPositive(Season season)
        {
            return m_Positives.TryGetValue(season, out ConfigEntry<int> entry)
                       ? entry.Value
                       : 0;
        }

        private void AddPositive(ConfigFile config, Season season, string key)
        {
            m_Positives[season] = config.Bind(
                SectionName,
                key,
                1,
                new ConfigDescription(
                    $"Positive seasonal traits for {season} while per-season counts are enabled. The remaining traits for that season become negative ones. Values above the season's normal total are clamped; missing candidates simply yield fewer traits.",
                    new AcceptableValueRange<int>(0, 10)));
        }
    }
}

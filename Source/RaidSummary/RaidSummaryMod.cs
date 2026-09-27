using HarmonyLib;
using RaidSummary.Settings;
using UnityEngine;
using Verse;

namespace RaidSummary
{
    public class RaidSummaryMod : Mod
    {
        public static RaidSummarySettings Settings;
        public RaidSummaryMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("IceFrost.RaidSummary");
            harmony.PatchAll();

            Settings = GetSettings<RaidSummarySettings>();
            Log.Message("[Raid Summary] initialized");
        }

        public override string SettingsCategory() => "Raid Summary";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            if(ModsConfig.BiotechActive)
                listing.CheckboxLabeled("RaidSummary.AutomaticShowXenotypeSettings".Translate(), ref Settings.autoShowXenotypes);

            listing.CheckboxLabeled("RaidSummary.AutomaticShowEquipmentSettings".Translate(), ref Settings.autoShowEquipment);
            listing.CheckboxLabeled("RaidSummary.AutomaticShowApparelSettings".Translate(), ref Settings.autoShowApparel);
            listing.CheckboxLabeled("RaidSummary.AutomaticShowAnimalsSettings".Translate(), ref Settings.autoShowAnimals);
            listing.CheckboxLabeled("RaidSummary.AutomaticShowMechanoidsSettings".Translate(), ref Settings.autoShowMechanoids);
            listing.CheckboxLabeled("RaidSummary.FriendliesSummarySettings".Translate(), ref Settings.createFriendliesReport);

            listing.End();
        }
    }
}
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
                listing.CheckboxLabeled("Automatically show xenotypes", ref Settings.autoShowXenotypes);

            listing.CheckboxLabeled("Automatically show equipment", ref Settings.autoShowEquipment);
            listing.CheckboxLabeled("Automatically show apparel", ref Settings.autoShowApparel);
            listing.CheckboxLabeled("Automatically show animals", ref Settings.autoShowAnimals);
            listing.CheckboxLabeled("Automatically show mechanoids", ref Settings.autoShowMechanoids);
            listing.CheckboxLabeled("Create friendlies summary reports", ref Settings.createFriendliesReport);

            listing.End();
        }
    }
}
using Verse;

namespace RaidSummary.Settings
{
    public class RaidSummarySettings : ModSettings
    {
        public bool autoShowXenotypes = true;
        public bool autoShowEquipment = true;
        public bool autoShowApparel = true;
        public bool autoShowAnimals = false;
        public bool autoShowMechanoids = true;
        public bool createFriendliesReport = false;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref autoShowXenotypes, "autoShowXenotypes", true);
            Scribe_Values.Look(ref autoShowEquipment, "autoShowEquipment", true);
            Scribe_Values.Look(ref autoShowApparel, "autoShowApparel", true);
            Scribe_Values.Look(ref autoShowAnimals, "autoShowAnimals", false);
            Scribe_Values.Look(ref autoShowMechanoids, "autoShowMechanoids", true);
            Scribe_Values.Look(ref createFriendliesReport, "createFriendliesReport", false);
        }
    }
}
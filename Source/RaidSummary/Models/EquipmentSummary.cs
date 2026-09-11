using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RaidSummary.Models
{
    public class EquipmentSummary : ThingSummary
    {
        private int biocodedCount = 0;

        public EquipmentSummary(ThingDef tDef) : base(tDef)
        {
        }

        public int BiocodedCount => biocodedCount;

        public void IncrementBiocode() {biocodedCount++;}

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref biocodedCount, "biocodedCount");
        }
    }
}
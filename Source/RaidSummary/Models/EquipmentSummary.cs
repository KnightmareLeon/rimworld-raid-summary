using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RaidSummary.Models
{
    public class EquipmentSummary : ThingSummary
    {
        private int bioCodedCount = 0;

        public EquipmentSummary(ThingDef tDef) : base(tDef)
        {
        }

        public int BiocodedCount => bioCodedCount;

        public void IncrementBiocode() {bioCodedCount++;}
    }
}
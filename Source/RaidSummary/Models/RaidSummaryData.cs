using System.Collections.Generic;
using Verse;

namespace RaidSummary.Models
{
    public class RaidSummaryData
    {
        public int PawnCount {get; set;}
        public Dictionary<ThingDef, EquipmentSummary> Equipment { get; set; }
            = new Dictionary<ThingDef, EquipmentSummary>();
    }
}
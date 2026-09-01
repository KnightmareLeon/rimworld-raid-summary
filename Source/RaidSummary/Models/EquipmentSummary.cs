using System.Collections.Generic;
using RimWorld;
using UnityEngine.PlayerLoop;
using Verse;

namespace RaidSummary.Models
{
    public class EquipmentSummary
    {
        public ThingDef EquipmentDef { get; set; }
        public Dictionary<QualityCategory, int> QualityCounts { get; set; }
            = new Dictionary<QualityCategory, int>();

        public int BiocodedCount = 0;

        public int Total = 0;
    }
}
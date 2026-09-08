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
        public Dictionary<ThingDef, int> MaterialCounts {get; set;}
            = new Dictionary<ThingDef, int>();
        public int BiocodedCount = 0;
        public int Total = 0;

        public float GetContentHeight()
        {
            float contentHeight = (QualityCounts.Count + MaterialCounts.Count) * (2f+ Text.LineHeight);
            contentHeight += (2f + Text.LineHeight) * 3; // Main Header, Total Header, Quality Header
            
            if (!MaterialCounts.NullOrEmpty())
                contentHeight += 2f + Text.LineHeight; // Material Header
            if (BiocodedCount > 0)
                contentHeight += 2f + Text.LineHeight; // Biocoded Header

            return contentHeight;
        }
    }
}
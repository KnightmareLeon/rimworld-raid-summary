using System.Collections.Generic;
using RimWorld;
using UnityEngine.PlayerLoop;
using Verse;

namespace RaidSummary.Models
{
    public class ApparelSummary
    {
        public ThingDef ApparelDef { get; set; }
        public Dictionary<QualityCategory, int> QualityCounts { get; set; }
            = new Dictionary<QualityCategory, int>();
        public Dictionary<ThingDef, int> MaterialCounts {get; set;}
            = new Dictionary<ThingDef, int>();
        public int Total = 0;

        public float GetContentHeight()
        {
            float contentHeight = (QualityCounts.Count + MaterialCounts.Count) * (2f+ Text.LineHeight);
            contentHeight += (2f + Text.LineHeight) * 3; // Main Header, Total Header, Quality Header
            
            if (!MaterialCounts.NullOrEmpty())
                contentHeight += 2f + Text.LineHeight; // Material Header

            return contentHeight;
        }
    }
}
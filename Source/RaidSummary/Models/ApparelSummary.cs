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
            return (QualityCounts.Count + MaterialCounts.Count) * 21f + 84f; 
        }
    }
}
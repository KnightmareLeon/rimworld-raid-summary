using System.Collections.Generic;
using RimWorld;
using Verse;

namespace RaidSummary.Models
{
    public abstract class ThingSummary
    {
        private ThingDef tDef;
        private Dictionary<QualityCategory, int> QualityCounts
            = new Dictionary<QualityCategory, int>();
        private Dictionary<ThingDef, int> MaterialCounts
            = new Dictionary<ThingDef, int>();

        private int total = 0;
        public int Total => total;
        
        public ThingSummary(ThingDef tDef)
        {
            this.tDef = tDef;
        }

        public Dictionary<QualityCategory, int>.Enumerator QualityEnumerator()
        {
            return QualityCounts.GetEnumerator();
        }

        public Dictionary<ThingDef, int>.Enumerator MaterialsEnumerator()
        {
            return MaterialCounts.GetEnumerator();
        }

        public void IncrementTotal() {total++;}

        public bool QualityExists(QualityCategory qCategory) => QualityCounts.ContainsKey(qCategory);

        public void InitializeQualityCount(QualityCategory qCategory) => QualityCounts[qCategory] = 0;

        public void IncrementQualityCount(QualityCategory qCategory) => QualityCounts[qCategory]++;

        public bool MaterialExists(ThingDef tDef) => MaterialCounts.ContainsKey(tDef);

        public void InitializeMaterialCount(ThingDef tDef) => MaterialCounts[tDef] = 0;

        public void IncrementMaterialCount(ThingDef tDef) => MaterialCounts[tDef]++;

        public bool MaterialNullOrEmpty() => MaterialCounts.NullOrEmpty();
    }
}
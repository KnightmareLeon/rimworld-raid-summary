using System.Collections.Generic;
using Verse;
using RimWorld;

namespace RaidSummary.Models
{
    public class RaidSummaryData
    {
        public int PawnCount {get; private set;}
        private readonly Dictionary<ThingDef, EquipmentSummary> equipmentSummaries
            = new Dictionary<ThingDef, EquipmentSummary>();
        private readonly Dictionary<ThingDef, ApparelSummary> apparelSummaries
            = new Dictionary<ThingDef, ApparelSummary>();
        private readonly Dictionary<XenotypeDef, int> xenotypeCounts
            = new Dictionary<XenotypeDef, int>();

        public RaidSummaryData(List<Pawn> pawns)
        {
            PawnCount = pawns.Count;

            foreach (Pawn pawn in pawns)
            {
                UpdateEquipmentSummaries(pawn.equipment?.Primary);
                UpdateApparelSummaries(pawn.apparel?.WornApparel);

                if (ModsConfig.BiotechActive)
                {
                    UpdateXenotypeCount(pawn.genes.Xenotype);
                }
            }
        }

        private void UpdateEquipmentSummaries(Thing equipment)
        {
            if (equipment == null)
                return;
            
            ThingDef equipmentDef = equipment.def;

            if (!equipmentSummaries.TryGetValue(equipmentDef, out EquipmentSummary equipmentSummary))
            {
                equipmentSummary = new EquipmentSummary
                {
                    EquipmentDef = equipmentDef
                };

                equipmentSummaries.Add(equipmentDef, equipmentSummary);
            }

            equipmentSummary.Total++;

            QualityCategory quality = QualityCategory.Normal;

            CompQuality compQuality = equipment.TryGetComp<CompQuality>();

            if (compQuality != null)
                quality = compQuality.Quality;

            if (!equipmentSummary.QualityCounts.ContainsKey(quality))
                equipmentSummary.QualityCounts[quality] = 0;

            equipmentSummary.QualityCounts[quality]++;

            ThingDef stuffDef = equipment?.Stuff;

            if(stuffDef != null)
            {
                if (!equipmentSummary.MaterialCounts.ContainsKey(stuffDef))
                    equipmentSummary.MaterialCounts[stuffDef] = 0;

                equipmentSummary.MaterialCounts[stuffDef]++;
            }

            CompBiocodable compBiocodable = equipment.TryGetComp<CompBiocodable>();

            if(compBiocodable != null)
                equipmentSummary.BiocodedCount += compBiocodable.Biocoded ? 1 : 0;
        }

        private void UpdateApparelSummaries(List<Apparel> wornApparel)
        {
            if (wornApparel.NullOrEmpty())
                return;
            
            foreach (Apparel apparel in wornApparel)
            {
                ThingDef apparelDef = apparel.def;

                if (!apparelSummaries.TryGetValue(apparelDef, out ApparelSummary apparelSummary))
                {
                    apparelSummary = new ApparelSummary
                    {
                        ApparelDef = apparelDef
                    };

                    apparelSummaries.Add(apparelDef, apparelSummary);
                }

                apparelSummary.Total++;

                QualityCategory quality = QualityCategory.Normal;

                CompQuality compQuality = apparel.TryGetComp<CompQuality>();

                if (compQuality != null)
                    quality = compQuality.Quality;

                if (!apparelSummary.QualityCounts.ContainsKey(quality))
                    apparelSummary.QualityCounts[quality] = 0;

                apparelSummary.QualityCounts[quality]++;

                ThingDef stuffDef = apparel?.Stuff;

                if(stuffDef != null)
                {
                    if (!apparelSummary.MaterialCounts.ContainsKey(stuffDef))
                        apparelSummary.MaterialCounts[stuffDef] = 0;

                    apparelSummary.MaterialCounts[stuffDef]++;
                }
            }

        }

        private void UpdateXenotypeCount(XenotypeDef xenotype)
        {
            if(!xenotypeCounts.ContainsKey(xenotype))
                xenotypeCounts[xenotype] = 0;

            xenotypeCounts[xenotype]++;
        }

        public Dictionary<ThingDef, EquipmentSummary>.Enumerator EquipmentSummariesEnumerator()
        {
            return equipmentSummaries.GetEnumerator();
        }

        public Dictionary<ThingDef, ApparelSummary>.Enumerator ApparelSummariesEnumerator()
        {
            return apparelSummaries.GetEnumerator();
        }

        public Dictionary<XenotypeDef, int>.Enumerator XenotypeCountsEnumerator()
        {
            return xenotypeCounts.GetEnumerator();
        }

        public int EquipmentSummariesCount() => equipmentSummaries.Count;
        public int ApparelSummariesCount() => apparelSummaries.Count;
        public int XenotypeTotal() => xenotypeCounts.Count;

        public float GetContentHeight()
        {
            float contentHeight = 0f;
            foreach(EquipmentSummary eqpSummary in equipmentSummaries.Values)
            {
                contentHeight += eqpSummary.GetContentHeight();
            }

            foreach(ApparelSummary apparelSummary in apparelSummaries.Values)
            {
                contentHeight += apparelSummary.GetContentHeight();
            }

            if(ModsConfig.BiotechActive)
            {
                contentHeight += xenotypeCounts.Count * 21f + 21f;
            }

            return contentHeight;
        }
    }
}
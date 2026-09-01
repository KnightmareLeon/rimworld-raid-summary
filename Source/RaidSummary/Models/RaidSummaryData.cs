using System.Collections.Generic;
using Verse;
using RimWorld;

namespace RaidSummary.Models
{
    public class RaidSummaryData
    {
        public int PawnCount {get; set;}
        private readonly Dictionary<ThingDef, EquipmentSummary> equipmentSummaries
            = new Dictionary<ThingDef, EquipmentSummary>();

        private readonly Dictionary<ThingDef, ApparelSummary> apparelSummaries
            = new Dictionary<ThingDef, ApparelSummary>();

        public void UpdateEquipmentSummaries(Thing equipment)
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

            CompBiocodable compBiocodable = equipment.TryGetComp<CompBiocodable>();

            if(compBiocodable != null)
                equipmentSummary.BiocodedCount += compBiocodable.Biocoded ? 1 : 0;
        }

        public void UpdateApparelSummaries(List<Apparel> wornApparel)
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
        public Dictionary<ThingDef, EquipmentSummary>.Enumerator EquipmentSummariesEnumerator()
        {
            return equipmentSummaries.GetEnumerator();
        }

        public Dictionary<ThingDef, ApparelSummary>.Enumerator ApparelSummariesEnumerator()
        {
            return apparelSummaries.GetEnumerator();
        }
    }
}
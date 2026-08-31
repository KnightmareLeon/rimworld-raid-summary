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

        public Dictionary<ThingDef, ApparelSummary> ApparelSummaries {get; set;}
            = new Dictionary<ThingDef, ApparelSummary>();

        public void UpdateEquipmentSummaries(Thing equipment)
        {
            if (equipment == null)
                return;
            
            ThingDef equipmentDef = equipment.def;

            equipmentSummaries.GetEnumerator();
            if (!equipmentSummaries.TryGetValue(equipmentDef, out EquipmentSummary equipmentSummary))
            {
                equipmentSummary = new EquipmentSummary
                {
                    EquipmentDef = equipmentDef
                };

                equipmentSummaries.Add(equipmentDef, equipmentSummary);
            }

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

        public Dictionary<ThingDef, EquipmentSummary>.Enumerator EquipmentSummariesEnumerator()
        {
            return equipmentSummaries.GetEnumerator();
        }
    }
}
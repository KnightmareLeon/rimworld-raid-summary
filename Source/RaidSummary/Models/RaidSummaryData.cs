using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Linq.Expressions;

namespace RaidSummary.Models
{
    public class RaidSummaryData
    {
        public int HumanPawnCount {get; private set;}
        public int AnimalPawnCount {get; private set;} = 0;
        public int MechanoidCount {get; private set;} = 0;

        private readonly Dictionary<ThingDef, EquipmentSummary> equipmentSummaries
            = new Dictionary<ThingDef, EquipmentSummary>();
        private readonly Dictionary<ThingDef, ApparelSummary> apparelSummaries
            = new Dictionary<ThingDef, ApparelSummary>();
        private readonly Dictionary<XenotypeDef, int> xenotypeCounts
            = new Dictionary<XenotypeDef, int>();
        private readonly Dictionary<PawnKindDef, int> animalCounts
            = new Dictionary<PawnKindDef, int>();
        private readonly Dictionary<PawnKindDef, int> mechanoidCounts
            = new Dictionary<PawnKindDef, int>();

        public RaidSummaryData(List<Pawn> pawns)
        {

            foreach (Pawn pawn in pawns)
            {
                if (pawn.IsAnimal)
                {
                    UpdateAnimalCount(pawn.kindDef);
                } 
                else if (pawn.RaceProps.IsMechanoid)
                {
                    UpdateMechanoidCount(pawn.kindDef);
                }
                else
                {
                    UpdateEquipmentSummaries(pawn.equipment?.Primary);
                    UpdateApparelSummaries(pawn.apparel?.WornApparel);

                    if (ModsConfig.BiotechActive)
                    {
                        UpdateXenotypeCount(pawn.genes.Xenotype);
                    }
                }
            }

            HumanPawnCount = pawns.Count - AnimalPawnCount - MechanoidCount;
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

        private void UpdateAnimalCount(PawnKindDef animalDef)
        {
            if(!animalCounts.ContainsKey(animalDef))
                animalCounts[animalDef] = 0;

            animalCounts[animalDef]++;

            AnimalPawnCount++;
        }

        private void UpdateMechanoidCount(PawnKindDef mechaDef)
        {
            if(!mechanoidCounts.ContainsKey(mechaDef))
                mechanoidCounts[mechaDef] = 0;

            mechanoidCounts[mechaDef]++;

            MechanoidCount++;
        }

        public Dictionary<ThingDef, EquipmentSummary>.Enumerator EquipmentSummariesEnumerator() => equipmentSummaries.GetEnumerator();
        public Dictionary<ThingDef, ApparelSummary>.Enumerator ApparelSummariesEnumerator() => apparelSummaries.GetEnumerator();
        public Dictionary<XenotypeDef, int>.Enumerator XenotypeCountsEnumerator() => xenotypeCounts.GetEnumerator();
        public Dictionary<PawnKindDef, int>.Enumerator AnimalCountsEnumerator() => animalCounts.GetEnumerator();
        public Dictionary<PawnKindDef, int>.Enumerator MechanoidCountsEnumerator() => mechanoidCounts.GetEnumerator();
    }
}
using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Linq.Expressions;

namespace RaidSummary.Models
{
    public class RaidSummaryData : IExposable
    {
        private int humanPawnCount;
        private int animalPawnCount = 0;
        private int mechanoidCount = 0;
        public int HumanPawnCount => humanPawnCount;
        public int AnimalPawnCount => animalPawnCount;
        public int MechanoidCount => mechanoidCount;

        private Dictionary<ThingDef, EquipmentSummary> equipmentSummaries
            = new Dictionary<ThingDef, EquipmentSummary>();
        private Dictionary<ThingDef, ApparelSummary> apparelSummaries
            = new Dictionary<ThingDef, ApparelSummary>();
        private Dictionary<XenotypeDef, int> xenotypeCounts
            = new Dictionary<XenotypeDef, int>();
        private Dictionary<PawnKindDef, int> animalCounts
            = new Dictionary<PawnKindDef, int>();
        private Dictionary<PawnKindDef, int> mechanoidCounts
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

            humanPawnCount = pawns.Count - AnimalPawnCount - MechanoidCount;
        }

        private void UpdateEquipmentSummaries(Thing equipment)
        {
            if (equipment == null)
                return;
            
            ThingDef equipmentDef = equipment.def;

            if (!equipmentSummaries.TryGetValue(equipmentDef, out EquipmentSummary equipmentSummary))
            {
                equipmentSummary = new EquipmentSummary(equipmentDef);

                equipmentSummaries.Add(equipmentDef, equipmentSummary);
            }

            equipmentSummary.IncrementTotal();

            QualityCategory quality = QualityCategory.Normal;

            CompQuality compQuality = equipment.TryGetComp<CompQuality>();

            if (compQuality != null)
                quality = compQuality.Quality;

            if (!equipmentSummary.QualityExists(quality))
                equipmentSummary.InitializeQualityCount(quality);

            equipmentSummary.IncrementQualityCount(quality);

            ThingDef stuffDef = equipment?.Stuff;

            if(stuffDef != null)
            {
                if (!equipmentSummary.MaterialExists(stuffDef))
                    equipmentSummary.InitializeMaterialCount(stuffDef);

                equipmentSummary.IncrementMaterialCount(stuffDef);
            }

            CompBiocodable compBiocodable = equipment.TryGetComp<CompBiocodable>();

            if(compBiocodable != null)
            {
                if(compBiocodable.Biocoded)
                    equipmentSummary.IncrementBiocode();
            }
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
                    apparelSummary = new ApparelSummary(apparelDef);

                    apparelSummaries.Add(apparelDef, apparelSummary);
                }

                apparelSummary.IncrementTotal();

                QualityCategory quality = QualityCategory.Normal;

                CompQuality compQuality = apparel.TryGetComp<CompQuality>();

                if (compQuality != null)
                    quality = compQuality.Quality;

                if (!apparelSummary.QualityExists(quality))
                    apparelSummary.InitializeQualityCount(quality);

                apparelSummary.IncrementQualityCount(quality);

                ThingDef stuffDef = apparel?.Stuff;

                if(stuffDef != null)
                {
                    if (!apparelSummary.MaterialExists(stuffDef))
                        apparelSummary.InitializeMaterialCount(stuffDef);

                    apparelSummary.IncrementMaterialCount(stuffDef);
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

            animalPawnCount++;
        }

        private void UpdateMechanoidCount(PawnKindDef mechaDef)
        {
            if(!mechanoidCounts.ContainsKey(mechaDef))
                mechanoidCounts[mechaDef] = 0;

            mechanoidCounts[mechaDef]++;

            mechanoidCount++;
        }

        public Dictionary<ThingDef, EquipmentSummary>.Enumerator EquipmentSummariesEnumerator() => equipmentSummaries.GetEnumerator();
        public Dictionary<ThingDef, ApparelSummary>.Enumerator ApparelSummariesEnumerator() => apparelSummaries.GetEnumerator();
        public Dictionary<XenotypeDef, int>.Enumerator XenotypeCountsEnumerator() => xenotypeCounts.GetEnumerator();
        public Dictionary<PawnKindDef, int>.Enumerator AnimalCountsEnumerator() => animalCounts.GetEnumerator();
        public Dictionary<PawnKindDef, int>.Enumerator MechanoidCountsEnumerator() => mechanoidCounts.GetEnumerator();
        public void ExposeData()
        {
            Scribe_Values.Look(ref humanPawnCount, "humanPawnCount");
            Scribe_Values.Look(ref animalPawnCount, "animalPawnCount");
            Scribe_Values.Look(ref mechanoidCount, "mechanoidCount");

            Scribe_Collections.Look(
                ref equipmentSummaries,
                "equipmentSummaries",
                LookMode.Def,
                LookMode.Deep
            );

            Scribe_Collections.Look(
                ref apparelSummaries,
                "apparelSummaries",
                LookMode.Def,
                LookMode.Deep
            );

            Scribe_Collections.Look(
                ref xenotypeCounts,
                "xenotypeCounts",
                LookMode.Def,
                LookMode.Value
            );

            Scribe_Collections.Look(
                ref animalCounts,
                "animalCounts",
                LookMode.Def,
                LookMode.Value
            );

            Scribe_Collections.Look(
                ref mechanoidCounts,
                "mechanoidCounts",
                LookMode.Def,
                LookMode.Value
            );
        }
    }
}
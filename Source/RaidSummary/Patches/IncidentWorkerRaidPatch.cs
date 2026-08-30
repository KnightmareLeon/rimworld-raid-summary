using System.Collections.Generic;
using HarmonyLib;
using RaidSummary.Models;
using RimWorld;
using Verse;

namespace RaidSummary.Patches
{
    [HarmonyPatch(typeof(IncidentWorker_Raid), "TryGenerateRaidInfo")]
    public static class IncidentWorkerRaidPatch
    {
        public static void Postfix(
            IncidentParms parms,
            List<Pawn> pawns,
            bool debugTest,
            bool __result)
        {
            if (!__result || debugTest || pawns == null)
                return;

            RaidSummaryData summary = new RaidSummaryData
            {
                PawnCount = pawns.Count
            };

            foreach (Pawn pawn in pawns)
            {
                Thing equipment = pawn.equipment?.Primary;

                if (equipment == null)
                    continue;

                ThingDef equipmentDef = equipment.def;

                if (!summary.Equipment.TryGetValue(equipmentDef, out EquipmentSummary equipmentSummary))
                {
                    equipmentSummary = new EquipmentSummary
                    {
                        EquipmentDef = equipmentDef
                    };

                    summary.Equipment.Add(equipmentDef, equipmentSummary);
                }

                QualityCategory quality = QualityCategory.Normal;

                CompQuality compQuality = equipment.TryGetComp<CompQuality>();

                if (compQuality != null)
                    quality = compQuality.Quality;

                if (!equipmentSummary.QualityCounts.ContainsKey(quality))
                    equipmentSummary.QualityCounts[quality] = 0;

                equipmentSummary.QualityCounts[quality]++;
            }

            Log.Message(
                $"[Raid Summary] Raid generated with {summary.PawnCount} pawns."
            );

            foreach(var (equipmentDef, equipmentSummary) in summary.Equipment)
            {
                Log.Message(
                    $"[Raid Summary] Total {equipmentDef.LabelCap}: {equipmentSummary.GetTotal()}"
                );

                foreach (var (quality, qualityCount) in equipmentSummary.QualityCounts)
                {
                    Log.Message(
                        $"[Raid Summary] Total {quality} {equipmentDef.LabelCap}: {qualityCount}"
                    );
                } 
            }

        }
    }
}
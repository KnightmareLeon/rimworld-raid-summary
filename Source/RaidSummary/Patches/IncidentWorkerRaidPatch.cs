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

                summary.UpdateEquipmentSummaries(equipment);

            }

            Log.Message(
                $"[Raid Summary] Raid generated with {summary.PawnCount} pawns."
            );

            using (var eqpSummayEnumerator = summary.EquipmentSummariesEnumerator())
            {
                while (eqpSummayEnumerator.MoveNext())
                {
                    ThingDef equipmentDef = eqpSummayEnumerator.Current.Key;
                    EquipmentSummary equipmentSummary = eqpSummayEnumerator.Current.Value;

                    Log.Message(
                        $"[Raid Summary] Total {equipmentDef.LabelCap}: {equipmentSummary.GetTotal()}"
                    );

                    foreach (var (quality, qualityCount) in equipmentSummary.QualityCounts)
                    {
                        Log.Message(
                            $"[Raid Summary] Total {quality} {equipmentDef.LabelCap}: {qualityCount}"
                        );
                    }

                    Log.Message(
                        $"[Raid Summary] Total Biocoded {equipmentDef.LabelCap}: {equipmentSummary.BiocodedCount}"
                    );
                }
            }

        }
    }
}
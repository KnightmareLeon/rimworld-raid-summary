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
                summary.UpdateEquipmentSummaries(pawn.equipment?.Primary);
                summary.UpdateApparelSummaries(pawn.apparel?.WornApparel);

                if (ModsConfig.BiotechActive)
                {
                    summary.UpdateXenotypeCount(pawn.genes.Xenotype);
                }
            }

            Log.Message(
                $"[Raid Summary] Raid generated with {summary.PawnCount} pawns."
            );

            if(ModsConfig.BiotechActive)
            {
                using (var xenSummaryEnumerator = summary.XenotypeCountsEnumerator())
                {
                    while (xenSummaryEnumerator.MoveNext())
                    {
                        Log.Message(
                            $"[Raid Summary] Total {xenSummaryEnumerator.Current.Key.LabelCap} pawns: {xenSummaryEnumerator.Current.Value}"
                        );
                    }
                }
            }


            using (var eqpSummaryEnumerator = summary.EquipmentSummariesEnumerator())
            {
                while (eqpSummaryEnumerator.MoveNext())
                {
                    ThingDef equipmentDef = eqpSummaryEnumerator.Current.Key;
                    EquipmentSummary equipmentSummary = eqpSummaryEnumerator.Current.Value;

                    Log.Message(
                        $"[Raid Summary] Total {equipmentDef.LabelCap}: {equipmentSummary.Total}"
                    );

                    foreach (var (quality, qualityCount) in equipmentSummary.QualityCounts)
                    {
                        Log.Message(
                            $"[Raid Summary] Total {quality} {equipmentDef.LabelCap}: {qualityCount}"
                        );
                    }

                    if(equipmentSummary.BiocodedCount > 0)
                    {
                        Log.Message(
                            $"[Raid Summary] Total Biocoded {equipmentDef.LabelCap}: {equipmentSummary.BiocodedCount}"
                        );
                    }

                    if(!equipmentSummary.MaterialCounts.NullOrEmpty())
                    {
                        foreach (var (materialDef, materialCount) in equipmentSummary.MaterialCounts)
                        {
                            Log.Message(
                                $"[Raid Summary] Total {materialDef.LabelCap} {equipmentDef.LabelCap}: {materialCount}"
                            );
                        }
                    }
                }
            }

            using (var appSummayEnumerator = summary.ApparelSummariesEnumerator())
            {
                while (appSummayEnumerator.MoveNext())
                {
                    ThingDef apparelDef = appSummayEnumerator.Current.Key;
                    ApparelSummary apparelSummary = appSummayEnumerator.Current.Value;

                    Log.Message(
                        $"[Raid Summary] Total {apparelDef.LabelCap}: {apparelSummary.Total}"
                    );

                    foreach (var (quality, qualityCount) in apparelSummary.QualityCounts)
                    {
                        Log.Message(
                            $"[Raid Summary] Total {quality} {apparelDef.LabelCap}: {qualityCount}"
                        );
                    }

                    if(!apparelSummary.MaterialCounts.NullOrEmpty())
                    {
                        foreach (var (materialDef, materialCount) in apparelSummary.MaterialCounts)
                        {
                            Log.Message(
                                $"[Raid Summary] Total {materialDef.LabelCap} {apparelDef.LabelCap}: {materialCount}"
                            );
                        }
                    }
                }
            }

        }
    }
}
using System.Collections.Generic;
using HarmonyLib;
using RaidSummary.Models;
using RaidSummary.UI;
using RaidSummary.Defs;
using RimWorld;
using Verse;

namespace RaidSummary.Patches
{
    [HarmonyPatch(typeof(MechClusterUtility), "SpawnCluster")]
    public static class MechClusterUtilityPatch
    {
        public static void Postfix(
            IntVec3 center,
            Map map,
            MechClusterSketch sketch,
            bool dropInPods,
            bool canAssaultColony,
            string questTag,
            List<Thing> __result)
        {
            if (__result.NullOrEmpty() || sketch == null)
                return;

            if (!map.IsPlayerHome)
                return;
            MechClusterSummaryData summary = new MechClusterSummaryData(Find.FactionManager.OfMechanoids, map, __result);

            MechClusterSummaryLetter letter =
                (MechClusterSummaryLetter)LetterMaker.MakeLetter(
                    MechClusterSummaryLetterDefOf.MechClusterSummaryLetter
                );

            letter.Initialize(summary);
            letter.Label = $"Mech Cluster Summary: {summary.GetFactionName()}";

            Find.LetterStack.ReceiveLetter(letter, delayTicks: 1);
        }
    }
}
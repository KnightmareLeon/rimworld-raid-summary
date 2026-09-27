using System.Collections.Generic;
using HarmonyLib;
using RaidSummary.Models;
using RaidSummary.UI;
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
                    SummaryLetterDefOf.MechClusterSummaryLetter
                );

            letter.Initialize(summary);
            letter.Label = "RaidSummary.MechClusterLetterLabel".Translate(
                summary.GetFactionName().Named("factionName")
            );

            Find.LetterStack.ReceiveLetter(letter, delayTicks: 1);
        }
    }
}
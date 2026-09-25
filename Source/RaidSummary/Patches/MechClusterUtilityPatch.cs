using System.Collections.Generic;
using HarmonyLib;
using RaidSummary.Models;
using RaidSummary.UI;
using RaidSummary.Utilities;
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

        }
    }
}
using System.Collections.Generic;
using HarmonyLib;
using RaidSummary.Models;
using RaidSummary.Settings;
using RaidSummary.UI;
using RaidSummary.Utilities;
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

            if(!parms.faction.HostileTo(Faction.OfPlayer) && !RaidSummaryMod.Settings.createFriendliesReport)
                return;

            RaidSummaryData summary = new RaidSummaryData(parms.faction, (Map)parms.target, pawns);

            RaidSummaryLetter letter =
                (RaidSummaryLetter)LetterMaker.MakeLetter(
                    RaidSummaryLetterDefOf.RaidSummaryLetter
                );

            letter.Initialize(summary);
            letter.Label = parms.faction.HostileTo(Faction.OfPlayer) ? "Raid Summary: " : "Friendlies Summary: ";
            letter.Label += parms.faction.Name;

            Find.LetterStack.ReceiveLetter(letter);
        }
    }
}
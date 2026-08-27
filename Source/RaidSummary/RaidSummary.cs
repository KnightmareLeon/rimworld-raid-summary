using HarmonyLib;
using Verse;

namespace RaidSummary
{
    public class RaidSummaryMod : Mod
    {
        public RaidSummaryMod(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("IceFrost.RaidSummary");
            harmony.PatchAll();

            Log.Message("[Raid Summary] initialized");
        }
    }
}
using RimWorld;
using Verse;

namespace RaidSummary.Defs
{
    [DefOf]
    public static class RaidSummaryLetterDefOf
    {
        public static LetterDef RaidSummaryLetter;

        static RaidSummaryLetterDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RaidSummaryLetterDefOf));
        }
    }
}
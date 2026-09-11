using RimWorld;
using Verse;

namespace RaidSummary.Utilities
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
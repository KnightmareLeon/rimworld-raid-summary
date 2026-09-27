using RimWorld;
using Verse;

namespace RaidSummary.UI
{
    [DefOf]
    public static class SummaryLetterDefOf
    {
        public static LetterDef RaidSummaryLetter;
        public static LetterDef MechClusterSummaryLetter;

        static SummaryLetterDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SummaryLetterDefOf));
        }
    }
}
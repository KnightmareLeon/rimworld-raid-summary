using RimWorld;
using Verse;

namespace RaidSummary.Defs
{
    [DefOf]
    public static class MechClusterSummaryLetterDefOf
    {
        public static LetterDef MechClusterSummaryLetter;

        static MechClusterSummaryLetterDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(MechClusterSummaryLetterDefOf));
        }
    }
}
using Verse;
using RaidSummary.Models;
using RimWorld;

namespace RaidSummary.UI
{
    public class MechClusterSummaryWindow : SummaryWindow
    {
        private readonly MechClusterSummaryData summary;
        private readonly TreeNode mechanoidNode = new TreeNode();
        private readonly TreeNode buildingNode = new TreeNode();
        public MechClusterSummaryWindow(MechClusterSummaryData summary)
        {
            this.summary = summary;

            mechanoidNode.SetOpen(OpenMask, RaidSummaryMod.Settings.autoShowMechanoids);
            buildingNode.SetOpen(OpenMask, true);

            doCloseX = true;
            draggable = true;
            absorbInputAroundWindow = false;
        }

        protected override void DrawContents(RaidSummaryListing listing)
        {
            int indentLevel = 0;

            Text.Font = GameFont.Medium;
            listing.DrawWindowTitle(
                summary.MechFaction,
                "RaidSummary.MechClusterSummaryWindowTitle".Translate(
                    summary.GetFactionName().Named("factionName")
                ),
                indentLevel);

            Text.Font = GameFont.Small; 
            listing.GapLine();

            listing.DrawLabel(
                "RaidSummary.DateAndTimeLabel".Translate(
                    summary.GetMechClusterDate()
                ),
                indentLevel
            );
            listing.Gap();

            listing.DrawLabel(
                "RaidSummary.MechanoidCountLabel".Translate(
                    summary.MechCount
                ),
                indentLevel
            );

            listing.DrawLabel(
                "RaidSummary.BuildingCountLabel".Translate(
                    summary.BuildingCount
                ),
                indentLevel
            );

            listing.DrawLabel(
                "RaidSummary.WallCountLabel".Translate(
                    summary.WallCount
                ),
                indentLevel
            );

            listing.DrawLabel(
                "RaidSummary.BarricadeCountLabel".Translate(
                    summary.BarricadeCount
                ),
                indentLevel
            );

            listing.GapLine();

            if(summary.MechCount > 0)
            {
                listing.DrawSection(
                    mechanoidNode,
                    "RaidSummary.MechanoidsSectionLabel".Translate(),
                    indentLevel,
                    OpenMask
                );

                if (mechanoidNode.IsOpen(OpenMask))
                {
                    using (var enumerator = summary.MechEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            PawnKindDef mechaDef = enumerator.Current.Key;
                            int mechaCount = enumerator.Current.Value;

                            listing.DrawLabelForPawnKind(mechaDef, indentLevel + 1, extraInfo:$": {mechaCount}");
                        }
                    }
                }
                listing.GapLine();
            }

            listing.DrawSection(
                buildingNode,
                "RaidSummary.BuildingSectionLabel".Translate(),
                indentLevel,
                OpenMask
            );

            if (buildingNode.IsOpen(OpenMask))
            {
                using (var enumerator = summary.BuildingEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        int buildingIndentLevel = indentLevel + 1;
                        ThingDef buildingDef = enumerator.Current.Key;
                        int buildingCount = enumerator.Current.Value;

                        listing.DrawLabelForThing(buildingDef, ref buildingIndentLevel, extraInfo:$": {buildingCount}");
                    }
                }
            }
            listing.GapLine();
        }
    }
}
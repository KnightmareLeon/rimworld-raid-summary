using UnityEngine;
using Verse;
using RaidSummary.Models;

namespace RaidSummary.UI
{
    public class RaidSummaryWindow : Window
    {
        private readonly RaidSummaryData summary;

        public RaidSummaryWindow(RaidSummaryData summary)
        {
            this.summary = summary;

            doCloseX = true;
            draggable = true;
            absorbInputAroundWindow = false;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(800f, 600f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();

            listing.Begin(inRect);

            listing.Label("Raid Summary");
            listing.Gap();

            listing.Label($"Pawns: {summary.PawnCount}");

            listing.GapLine();

            listing.Label("Equipment");

            listing.End();
        }
    }
}
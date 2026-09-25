using UnityEngine;
using Verse;

namespace RaidSummary.UI
{

    public abstract class SummaryWindow : Window
    {
        private Vector2 scrollPosition = Vector2.zero;
        private float viewHeight;

        protected const int OpenMask = 1;
        protected abstract void DrawContents(RaidSummaryListing listing);
        public override void DoWindowContents(Rect inRect)
        {
            float width = inRect.width - 16f;

            Rect viewRect = new Rect(0f, 0f, width, viewHeight);

            Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

            Rect listingRect = new Rect(0f, 0f, viewRect.width, 6900f);
            RaidSummaryListing listing = new RaidSummaryListing();

            listing.Begin(listingRect);
            DrawContents(listing);
            listing.End();

            if (Event.current.type == EventType.Layout)
                viewHeight = listing.CurHeight;

            Widgets.EndScrollView();
        }
    }
}
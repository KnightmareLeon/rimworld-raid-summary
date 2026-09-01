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

        }
    }
}
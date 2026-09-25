using RaidSummary.Models;
using Verse;

namespace RaidSummary.UI
{
    public class MechClusterSummaryLetter : Letter
    {
        private MechClusterSummaryData summary;

        public void Initialize(MechClusterSummaryData summary)
        {
            this.summary = summary;
        }

        public override void OpenLetter()
        {
            Find.WindowStack.Add(new MechClusterSummaryWindow(summary));
            Find.LetterStack.RemoveLetter(this);
        }

        protected override string GetMouseoverText()
        {
            return $"Generated summary report for {summary.GetFactionName(applyTag: true)}'s mech cluster on {summary.GetMechClusterDate()}.";
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref summary, "summary");
        }
    }
}
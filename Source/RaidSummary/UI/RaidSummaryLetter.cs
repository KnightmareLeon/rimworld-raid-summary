using RaidSummary.Models;
using Verse;

namespace RaidSummary.UI
{
    public class RaidSummaryLetter : Letter
    {
        private RaidSummaryData summary;

        public void Initialize(RaidSummaryData summary)
        {
            this.summary = summary;
        }

        public override void OpenLetter()
        {
            Find.WindowStack.Add(new RaidSummaryWindow(summary));
            Find.LetterStack.RemoveLetter(this);
        }

        protected override string GetMouseoverText()
        {
            string incident = summary.IsFactionEnemy() ? "raid" : "reinforcement";
            return $"Generated summary report for {summary.Faction.Name.ApplyTag(summary.Faction).CapitalizeFirst()}'s {incident} on {summary.GetRaidDate()}.";
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref summary, "summary");
        }
    }
}
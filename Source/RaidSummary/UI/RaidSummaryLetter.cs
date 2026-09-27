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
            TaggedString incident =
                summary.IsFactionEnemy() ? 
                "RaidSummary.raid".Translate() : 
                "RaidSummary.reinforcement".Translate();

            return $"{summary.GetFactionName(applyTag: true)}: {"RaidSummary.RaidSummaryLetterText".Translate(incident.Named("raidOrReinforcement"),summary.GetRaidDate().Named("dateAndTime"))}";
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref summary, "summary");
        }
    }
}
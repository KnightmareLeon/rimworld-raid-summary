using UnityEngine;
using Verse;
using RaidSummary.Models;
using RimWorld;

namespace RaidSummary.UI
{
    public class RaidSummaryWindow : Window
    {
        private readonly RaidSummaryData summary;
        private Vector2 scrollPosition = Vector2.zero;
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

        public void DrawEquipment(Listing_Standard listing, ThingDef eqpDef, EquipmentSummary eqpSummary)
        {
            listing.Label($"    {eqpDef.LabelCap}");

            listing.Label($"        Total: {eqpSummary.Total}");

            if(eqpSummary.BiocodedCount > 0)
            {
                listing.Label(
                    $"      Biocoded:  {eqpSummary.BiocodedCount}"
                );

            }

            listing.Label($"        By Quality:");

            foreach (var (quality, qualityCount) in eqpSummary.QualityCounts)
            {
                listing.Label(
                    $"          {quality}:  {qualityCount}"
                );
            }

            if(!eqpSummary.MaterialCounts.NullOrEmpty())
            {
                listing.Label(
                    $"        By Material:"
                );

                foreach (var (materialDef, materialCount) in eqpSummary.MaterialCounts)
                {
                    listing.Label(
                        $"          {materialDef.LabelCap}: {materialCount}"
                    );
                }
            }
        }

        public void DrawApparel(Listing_Standard listing, ThingDef appDef, ApparelSummary appSummary)
        {
            listing.Label($"    {appDef.LabelCap}");

            listing.Label($"        Total: {appSummary.Total}");

            listing.Label($"        By Quality:");

            foreach (var (quality, qualityCount) in appSummary.QualityCounts)
            {
                listing.Label(
                    $"          {quality}:  {qualityCount}"
                );
            }

            if(!appSummary.MaterialCounts.NullOrEmpty())
            {
                listing.Label(
                    $"        By Material:"
                );

                foreach (var (materialDef, materialCount) in appSummary.MaterialCounts)
                {
                    listing.Label(
                        $"          {materialDef.LabelCap}: {materialCount}"
                    );
                }
            }
        }

        public float ComputeContentHeight()
        {
            float contentHeight = 0f;
            contentHeight += 30f; // Title heading
            contentHeight += 30f; // Equipment heading
            contentHeight += 30f; // Apparel heading
            if(ModsConfig.BiotechActive)
            {
                contentHeight += 30f; // Xenotypes heading
                contentHeight += summary.XenotypeTotal() * 24f;
            }

            contentHeight += summary.GetContentHeight();

            return contentHeight;
        }

        public override void DoWindowContents(Rect inRect)
        {
            float contentHeight = ComputeContentHeight();

            Rect viewRect = new Rect(
                0f,
                0f,
                inRect.width - 20f,
                contentHeight
            );

            Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

            Listing_Standard listing = new Listing_Standard();

            listing.Begin(viewRect);

            listing.Label("Raid Summary");
            listing.Gap();

            listing.Label($"Pawns: {summary.PawnCount}");
            
            listing.GapLine();

            if(ModsConfig.BiotechActive)
            {
                listing.Label("Xenotypes");

                using(var xenoTypeEnumerator = summary.XenotypeCountsEnumerator())
                {
                    while (xenoTypeEnumerator.MoveNext())
                    {
                        XenotypeDef xenoDef = xenoTypeEnumerator.Current.Key;
                        int xenoCount = xenoTypeEnumerator.Current.Value;

                        listing.Label($"    {xenoDef.LabelCap}: {xenoCount}");
                    }

                }

                listing.GapLine();
            }

            listing.Label("Equipment");

            using(var eqpSummaryEnumerator = summary.EquipmentSummariesEnumerator())
            {
                while (eqpSummaryEnumerator.MoveNext())
                {
                    ThingDef eqpDef = eqpSummaryEnumerator.Current.Key;
                    EquipmentSummary eqpSummary = eqpSummaryEnumerator.Current.Value;

                    DrawEquipment(listing, eqpDef, eqpSummary);
                }

            }

            listing.GapLine();

            listing.Label("Apparel");

            using(var appSummarEnumerator = summary.ApparelSummariesEnumerator())
            {
                while (appSummarEnumerator.MoveNext())
                {
                    ThingDef appDef = appSummarEnumerator.Current.Key;
                    ApparelSummary appSummary = appSummarEnumerator.Current.Value;

                    DrawApparel(listing, appDef, appSummary);
                }

            }

            listing.End();

            Widgets.EndScrollView();
        }
    }
}
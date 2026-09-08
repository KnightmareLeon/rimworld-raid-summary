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
                    $"          Biocoded:  {eqpSummary.BiocodedCount}"
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

            contentHeight += (2f + Text.LineHeight) * 5; // Title + Equipment + Apparel + Total Human and Animal Headings

            if(ModsConfig.BiotechActive)
            {
                contentHeight += 2f + Text.LineHeight; // Xenotypes heading
                contentHeight += 15f; // GapLine Height
            }
            if(summary.AnimalPawnCount > 0)
            {
                contentHeight += 2f + Text.LineHeight; // Animals heading
                contentHeight += 15f; // GapLine Height
            }

            contentHeight += summary.GetContentHeight(); // All content aside headers
            contentHeight += 15f * 2; // GapLine Height
            contentHeight += 14f; // Gap Height

            contentHeight += 2f + Text.LineHeight; // To be removed later, for testing purpose if height computation is correct.
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

            listing.Label($"Human Pawns: {summary.HumanPawnCount}");
            listing.Label($"Animal Pawns: {summary.AnimalPawnCount}");
            
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

            if(summary.AnimalPawnCount > 0)
            {
                listing.GapLine();

                listing.Label("Animals");

                using(var animalCountsEnum = summary.AnimalCountsEnumerator())
                {
                    while (animalCountsEnum.MoveNext())
                    {
                        PawnKindDef animalDef = animalCountsEnum.Current.Key;
                        int animalCount = animalCountsEnum.Current.Value;

                        listing.Label($"    {animalDef.LabelCap}: {animalCount}");
                    }
                }
            }

            listing.Label("If you are seeing this, you got the content height correct.");

            listing.End();

            Widgets.EndScrollView();
        }
    }
}
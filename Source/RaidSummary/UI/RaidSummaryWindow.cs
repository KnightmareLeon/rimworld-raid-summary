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
        private float viewHeight;

        private const int OpenMask = 1;

        private readonly TreeNode xenotypeNode = new TreeNode();
        private readonly TreeNode equipmentNode = new TreeNode();
        private readonly TreeNode apparelNode = new TreeNode();
        private readonly TreeNode animalNode = new TreeNode();

        public RaidSummaryWindow(RaidSummaryData summary)
        {
            this.summary = summary;

            xenotypeNode.SetOpen(OpenMask, true);
            equipmentNode.SetOpen(OpenMask, false);
            apparelNode.SetOpen(OpenMask, false);
            animalNode.SetOpen(OpenMask, false);

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

        private void DrawEquipment(RaidSummaryListing listing, ThingDef eqpDef, EquipmentSummary eqpSummary, int indentLevel)
        {
            int eqpIndentLevel = indentLevel;
            listing.DrawLabelForThing(eqpDef, ref eqpIndentLevel);

            listing.DrawLabel($"Total: {eqpSummary.Total}", eqpIndentLevel + 1);

            if (eqpSummary.BiocodedCount > 0)
                listing.DrawLabel($"Biocoded: {eqpSummary.BiocodedCount}", eqpIndentLevel + 1);

            listing.DrawLabel("By Quality:", eqpIndentLevel + 1);

            foreach (var (quality, qualityCount) in eqpSummary.QualityCounts)
                listing.DrawLabel($"{quality}: {qualityCount}", eqpIndentLevel + 2);

            if (!eqpSummary.MaterialCounts.NullOrEmpty())
            {
                listing.DrawLabel("By Material:", eqpIndentLevel + 1);
                foreach (var (materialDef, materialCount) in eqpSummary.MaterialCounts)
                {
                    int matIndentLevel = eqpIndentLevel + 3;
                    listing.DrawLabelForThing(materialDef, ref matIndentLevel, extraInfo:$": {materialCount}");
                }
            }
        }

        private void DrawApparel(RaidSummaryListing listing, ThingDef appDef, ApparelSummary appSummary, int indentLevel)
        {
            listing.DrawLabel(appDef.LabelCap, indentLevel);

            listing.DrawLabel($"Total: {appSummary.Total}", indentLevel + 1);

            listing.DrawLabel("By Quality:", indentLevel + 1);

            foreach (var (quality, qualityCount) in appSummary.QualityCounts)
                listing.DrawLabel($"{quality}: {qualityCount}", indentLevel + 2);

            if (!appSummary.MaterialCounts.NullOrEmpty())
            {
                listing.DrawLabel("By Material:", indentLevel + 1);

                foreach (var (materialDef, materialCount) in appSummary.MaterialCounts)
                    listing.DrawLabel($"{materialDef.LabelCap}: {materialCount}", indentLevel + 2);
            }
        }

        private void DrawContents(RaidSummaryListing listing)
        {
            listing.DrawLabel("Raid Summary", 0);

            listing.Gap();

            listing.DrawLabel($"Human Pawns: {summary.HumanPawnCount}", 0);

            listing.DrawLabel($"Animal Pawns: {summary.AnimalPawnCount}", 0);

            listing.GapLine();

            if (ModsConfig.BiotechActive)
            {
                listing.DrawSection(xenotypeNode, "Xenotypes", 0, OpenMask);

                if (xenotypeNode.IsOpen(OpenMask))
                {
                    using (var enumerator = summary.XenotypeCountsEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            XenotypeDef xenoDef = enumerator.Current.Key;
                            int xenoCount = enumerator.Current.Value;

                            listing.DrawLabel($"{xenoDef.LabelCap}: {xenoCount}", 1);
                        }
                    }
                }

                listing.GapLine();
            }

            listing.DrawSection(equipmentNode, "Equipment", 0, OpenMask);

            if (equipmentNode.IsOpen(OpenMask))
            {
                using (var enumerator = summary.EquipmentSummariesEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ThingDef eqpDef =enumerator.Current.Key;

                        EquipmentSummary eqpSummary = enumerator.Current.Value;

                        DrawEquipment(listing,eqpDef,eqpSummary, 1);
                    }
                }
            }

            listing.GapLine();

            listing.DrawSection(apparelNode,"Apparel",0,OpenMask);

            if (apparelNode.IsOpen(OpenMask))
            {
                using (var enumerator = summary.ApparelSummariesEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ThingDef appDef = enumerator.Current.Key;
                        ApparelSummary appSummary = enumerator.Current.Value;

                        DrawApparel(listing, appDef, appSummary,1);
                    }
                }
            }

            if (summary.AnimalPawnCount > 0)
            {
                listing.GapLine();

                listing.DrawSection(animalNode, "Animals", 0, OpenMask);

                if (animalNode.IsOpen(OpenMask))
                {
                    using (var enumerator = summary.AnimalCountsEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            PawnKindDef animalDef = enumerator.Current.Key;
                            int animalCount =enumerator.Current.Value;

                            listing.DrawLabel($"{animalDef.LabelCap}: {animalCount}", 1);
                        }
                    }
                }
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            float width = inRect.width - 16f;

            Rect viewRect = new Rect(
                0f,
                0f,
                width,
                viewHeight
            );

            Widgets.BeginScrollView(
                inRect,
                ref scrollPosition,
                viewRect
            );

            Rect listingRect = new Rect(
                0f,
                0f,
                viewRect.width,
                99999f
            );

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
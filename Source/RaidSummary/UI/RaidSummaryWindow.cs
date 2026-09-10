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
        private readonly ThingRootSummaryNode rootEquipmentNode = new ThingRootSummaryNode();
        private readonly ThingRootSummaryNode rootApparelNode = new ThingRootSummaryNode();
        private readonly TreeNode animalNode = new TreeNode();
        private readonly TreeNode mechanoidNode = new TreeNode();

        public RaidSummaryWindow(RaidSummaryData summary)
        {
            this.summary = summary;

            xenotypeNode.SetOpen(OpenMask, true);
            rootEquipmentNode.SetOpen(OpenMask, false);
            rootEquipmentNode.SetOpen(OpenMask, false);
            animalNode.SetOpen(OpenMask, false);

            using (var enumerator = summary.EquipmentSummariesEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ThingDef eqpDef = enumerator.Current.Key;
                    rootEquipmentNode.AddThingSummaryNode(eqpDef, OpenMask);
                }
            }

            using (var enumerator = summary.ApparelSummariesEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ThingDef appDef = enumerator.Current.Key;
                    rootApparelNode.AddThingSummaryNode(appDef, OpenMask);
                }
            }

            doCloseX = true;
            draggable = true;
            absorbInputAroundWindow = false;
        }

        public override Vector2 InitialSize
        {
            get{return new Vector2(800f, 600f);}
        }

        private void DrawEquipment(RaidSummaryListing listing, ThingDef eqpDef, EquipmentSummary eqpSummary, int indentLevel)
        {
            int eqpIndentLevel = indentLevel;

            ThingSummaryNode eqpNode = rootEquipmentNode.GetThingSummaryNode(eqpDef);

            listing.DrawSectionForThing(eqpNode, eqpDef, ref eqpIndentLevel, OpenMask, extraInfo: $": {eqpSummary.Total}");
            
            if (eqpNode.IsOpen(OpenMask))
            {
                if (eqpSummary.BiocodedCount > 0)
                    listing.DrawLabel($"Biocoded: {eqpSummary.BiocodedCount}", eqpIndentLevel + 1);

                listing.DrawSection(eqpNode.QualitiesNode, "By Quality:", eqpIndentLevel + 1, OpenMask);

                if(eqpNode.QualitiesNode.IsOpen(OpenMask))
                {
                    foreach (var (quality, qualityCount) in eqpSummary.QualityCounts)
                        listing.DrawLabel($"{quality}: {qualityCount}", eqpIndentLevel + 2);
                }

                if (!eqpSummary.MaterialCounts.NullOrEmpty())
                {
                    listing.DrawSection(eqpNode.MaterialsNode, "By Material:", eqpIndentLevel + 1, OpenMask);

                    if(eqpNode.MaterialsNode.IsOpen(OpenMask))
                    {
                        foreach (var (materialDef, materialCount) in eqpSummary.MaterialCounts)
                        {
                            int matIndentLevel = eqpIndentLevel + 3;
                            listing.DrawLabelForThing(materialDef, ref matIndentLevel, extraInfo:$": {materialCount}");
                        }
                    }

                }
            }
        }

        private void DrawApparel(RaidSummaryListing listing, ThingDef appDef, ApparelSummary appSummary, int indentLevel)
        {
            int appIndentLevel = indentLevel;
            ThingSummaryNode appNode = rootApparelNode.GetThingSummaryNode(appDef); 
            listing.DrawSectionForThing(appNode, appDef, ref appIndentLevel, OpenMask, extraInfo: $": {appSummary.Total}");

            if(appNode.IsOpen(OpenMask))
            {
                listing.DrawSection(appNode.QualitiesNode, "By Quality:", appIndentLevel + 1, OpenMask);

                if(appNode.QualitiesNode.IsOpen(OpenMask))
                {
                    foreach (var (quality, qualityCount) in appSummary.QualityCounts)
                        listing.DrawLabel($"{quality}: {qualityCount}", appIndentLevel + 2);
                }

                if (!appSummary.MaterialCounts.NullOrEmpty())
                {
                    listing.DrawSection(appNode.MaterialsNode, "By Material:", appIndentLevel + 1, OpenMask);

                    if(appNode.MaterialsNode.IsOpen(OpenMask))
                    {
                        foreach (var (materialDef, materialCount) in appSummary.MaterialCounts)
                        {
                            int matIndentLevel = appIndentLevel + 3;
                            listing.DrawLabelForThing(materialDef, ref matIndentLevel, extraInfo:$": {materialCount}");
                        }
                    }
                }
            }

        }

        private void DrawContents(RaidSummaryListing listing)
        {
            int indentLevel = 0;

            listing.DrawLabel("Raid Summary", indentLevel);

            listing.Gap();

            listing.DrawLabel($"Human Pawns: {summary.HumanPawnCount}", indentLevel);
            listing.DrawLabel($"Animal Pawns: {summary.AnimalPawnCount}", indentLevel);
            listing.DrawLabel($"Mechanoid Pawns: {summary.MechanoidCount}", indentLevel);

            listing.GapLine();

            if (ModsConfig.BiotechActive && summary.HumanPawnCount > 0)
            {
                listing.DrawSection(xenotypeNode, "Xenotypes", indentLevel, OpenMask);

                if (xenotypeNode.IsOpen(OpenMask))
                {
                    using (var enumerator = summary.XenotypeCountsEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            XenotypeDef xenoDef = enumerator.Current.Key;
                            int xenoCount = enumerator.Current.Value;

                            listing.DrawLabelForXenotype(xenoDef, indentLevel + 1, extraInfo: $": {xenoCount}");
                        }
                    }
                }

                listing.GapLine();
            }

            if (summary.HumanPawnCount > 0)
            {
                listing.DrawSection(rootEquipmentNode, "Equipment", indentLevel, OpenMask);

                if (rootEquipmentNode.IsOpen(OpenMask))
                {
                    using (var enumerator = summary.EquipmentSummariesEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            ThingDef eqpDef = enumerator.Current.Key;

                            EquipmentSummary eqpSummary = enumerator.Current.Value;

                            DrawEquipment(listing, eqpDef, eqpSummary, indentLevel + 1);
                        }
                    }
                }

                listing.GapLine();

                listing.DrawSection(rootApparelNode, "Apparel", indentLevel, OpenMask);

                if (rootApparelNode.IsOpen(OpenMask))
                {
                    using (var enumerator = summary.ApparelSummariesEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            ThingDef appDef = enumerator.Current.Key;
                            ApparelSummary appSummary = enumerator.Current.Value;

                            DrawApparel(listing, appDef, appSummary, indentLevel + 1);
                        }
                    }
                }
                listing.GapLine();
            }

            if (summary.AnimalPawnCount > 0)
            {

                listing.DrawSection(animalNode, "Animals", indentLevel, OpenMask);

                if (animalNode.IsOpen(OpenMask))
                {
                    using (var enumerator = summary.AnimalCountsEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            PawnKindDef animalDef = enumerator.Current.Key;
                            int animalCount = enumerator.Current.Value;

                            listing.DrawLabelForPawnKind(animalDef, indentLevel + 1, extraInfo:$": {animalCount}");
                        }
                    }
                }
                listing.GapLine();
            }

            if(summary.MechanoidCount > 0)
            {
                listing.DrawSection(mechanoidNode, "Mechanoids", indentLevel, OpenMask);

                if (mechanoidNode.IsOpen(OpenMask))
                {
                    using (var enumerator = summary.MechanoidCountsEnumerator())
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
                6900f
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
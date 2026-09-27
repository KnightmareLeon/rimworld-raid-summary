using Verse;
using RaidSummary.Models;
using RimWorld;
using RaidSummary.Utilities;

namespace RaidSummary.UI
{
    public class RaidSummaryWindow : SummaryWindow
    {
        private readonly RaidSummaryData summary;
        private readonly TreeNode xenotypeNode = new TreeNode();
        private readonly ThingRootSummaryNode rootEquipmentNode = new ThingRootSummaryNode();
        private readonly ThingRootSummaryNode rootApparelNode = new ThingRootSummaryNode();
        private readonly TreeNode animalNode = new TreeNode();
        private readonly TreeNode mechanoidNode = new TreeNode();

        public RaidSummaryWindow(RaidSummaryData summary)
        {
            this.summary = summary;

            xenotypeNode.SetOpen(OpenMask, RaidSummaryMod.Settings.autoShowXenotypes);
            rootEquipmentNode.SetOpen(OpenMask, RaidSummaryMod.Settings.autoShowEquipment);
            rootApparelNode.SetOpen(OpenMask, RaidSummaryMod.Settings.autoShowApparel);
            animalNode.SetOpen(OpenMask, RaidSummaryMod.Settings.autoShowAnimals);
            mechanoidNode.SetOpen(OpenMask, RaidSummaryMod.Settings.autoShowMechanoids);

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

        private void DrawEquipment(RaidSummaryListing listing, ThingDef eqpDef, EquipmentSummary eqpSummary, int indentLevel)
        {
            int eqpIndentLevel = indentLevel;

            ThingSummaryNode eqpNode = rootEquipmentNode.GetThingSummaryNode(eqpDef);

            listing.DrawSectionForThing(eqpNode, eqpDef, ref eqpIndentLevel, OpenMask, extraInfo: $": {eqpSummary.Total}");
            
            if (eqpNode.IsOpen(OpenMask))
            {
                if (eqpSummary.BiocodedCount > 0)
                    listing.DrawLabel(
                        "RaidSummary.BiocodedCountLabel".Translate(
                            eqpSummary.BiocodedCount
                        ),
                        eqpIndentLevel + 1
                    );

                listing.DrawSection(
                    eqpNode.QualitiesNode,
                    "RaidSummary.QualitiesSectionLabel".Translate(),
                    eqpIndentLevel + 1,
                    OpenMask
                );

                if(eqpNode.QualitiesNode.IsOpen(OpenMask))
                {
                    using(var enumerator = eqpSummary.QualityEnumerator())
                    {
                        while(enumerator.MoveNext())
                        {
                            QualityCategory quality = enumerator.Current.Key;
                            int qualityCount = enumerator.Current.Value;
                            listing.DrawLabel($"{quality}: {qualityCount}", eqpIndentLevel + 2);
                        }
                    }
                }

                if (!eqpSummary.MaterialNullOrEmpty())
                {
                    listing.DrawSection(
                        eqpNode.MaterialsNode,
                        "RaidSummary.MaterialSectionLabel".Translate(),
                        eqpIndentLevel + 1,
                        OpenMask
                    );

                    if(eqpNode.MaterialsNode.IsOpen(OpenMask))
                    {
                        using(var enumerator = eqpSummary.MaterialsEnumerator())
                        {
                            while(enumerator.MoveNext())
                            {
                                int materialIndent = eqpIndentLevel + 2;
                                ThingDef material = enumerator.Current.Key;
                                int materialCount = enumerator.Current.Value;
                                listing.DrawLabelForThing(material, ref materialIndent, $": {materialCount}");
                            }
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
                listing.DrawSection(
                    appNode.QualitiesNode,
                    "RaidSummary.QualitiesSectionLabel".Translate(),
                    appIndentLevel + 1,
                    OpenMask
                );

                if(appNode.QualitiesNode.IsOpen(OpenMask))
                {
                    using(var enumerator = appSummary.QualityEnumerator())
                    {
                        while(enumerator.MoveNext())
                        {
                            QualityCategory quality = enumerator.Current.Key;
                            int qualityCount = enumerator.Current.Value;
                            listing.DrawLabel($"{quality}: {qualityCount}", appIndentLevel + 2);
                        }
                    }
                }

                if (!appSummary.MaterialNullOrEmpty())
                {
                    listing.DrawSection(
                        appNode.MaterialsNode,
                        "RaidSummary.MaterialSectionLabel".Translate(),
                        appIndentLevel + 1,
                        OpenMask
                    );

                    if(appNode.MaterialsNode.IsOpen(OpenMask))
                    {
                        using(var enumerator = appSummary.MaterialsEnumerator())
                        {
                            while(enumerator.MoveNext())
                            {
                                int materialIndent = appIndentLevel + 2;
                                ThingDef material = enumerator.Current.Key;
                                int materialCount = enumerator.Current.Value;
                                listing.DrawLabelForThing(material, ref materialIndent, $": {materialCount}");
                            }
                        }
                    }
                }
            }

        }

        protected override void DrawContents(RaidSummaryListing listing)
        {
            int indentLevel = 0;

            Text.Font = GameFont.Medium;
            TaggedString raidOrFriendlies = summary.IsFactionEnemy() ? "RaidSummary.Raid".Translate() : "RaidSummary.Friendlies".Translate();
            listing.DrawWindowTitle(
                summary.Faction,
                "RaidSummaryWindowTitle".Translate(
                    summary.GetFactionName().Named("factionName"),
                    raidOrFriendlies.Named("raidOrFriendlies")), 
                indentLevel
            );

            Text.Font = GameFont.Small; 
            listing.GapLine();

            listing.DrawLabel(
                "RaidSummary.DateAndTimeLabel".Translate(
                    summary.GetRaidDate()),
                indentLevel
            );

            listing.DrawLabel(
                "RaidSummary.StrategyLabel".Translate(
                    summary.GetRaidStrategySummary()), 
                indentLevel
            );

            string arrivalMode = summary.ArrivalMode != null ?
                (TaggedString)Utility.DefNameWordSeparator(summary.ArrivalMode.defName) : 
                "RaidSummary.ArrivalModeNull".Translate();
            listing.DrawLabel(
                "RaidSummary.ArrivalModeLabel".Translate(
                    arrivalMode),
                indentLevel
            );

            listing.Gap();

            listing.DrawLabel(
                "RaidSummary.TotalPawnCountLabel".Translate(
                    summary.TotalPawnCount
                ),
                indentLevel
            );

            if (summary.HumanPawnCount > 0)
                listing.DrawLabel(
                    "RaidSummary.HumanPawnLabel".Translate(
                        summary.HumanPawnCount
                    ),
                    indentLevel
                );

            if (summary.AnimalPawnCount > 0)
                listing.DrawLabel(
                    "RaidSummary.AnimalCountLabel".Translate(
                        summary.AnimalPawnCount
                    ),
                    indentLevel
                );

            if (summary.MechanoidCount > 0)
                listing.DrawLabel(
                    "RaidSummary.MechanoidCountLabel".Translate(
                        summary.MechanoidCount
                    ),
                    indentLevel
                );
            
            if (summary.ShamblerCount > 0)
                listing.DrawLabel(
                    "RaidSummary.ShamblerCountLabel".Translate(
                        summary.ShamblerCount
                    ),
                    indentLevel
                );

            listing.GapLine();

            if (ModsConfig.BiotechActive && summary.HumanPawnCount > 0)
            {
                listing.DrawSection(xenotypeNode, "RaidSummary.XenotypesSectionLabel".Translate(), indentLevel, OpenMask);

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

            if (!summary.EquipmentSummariesNullOrEmpty())
            {
                listing.DrawSection(rootEquipmentNode, "RaidSummary.EquipmentSectionLabel".Translate(), indentLevel, OpenMask);

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
            }

            if (!summary.ApparelSummariesNullOrEmpty())
            {
                listing.DrawSection(
                    rootApparelNode,
                    "RaidSummary.ApparelSectionLabel".Translate(),
                    indentLevel,
                    OpenMask
                );

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

                listing.DrawSection(
                    animalNode,
                    "RaidSummary.AnimalsSectionLabel".Translate(),
                    indentLevel,
                    OpenMask
                );

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
                listing.DrawSection(
                    mechanoidNode,
                    "RaidSummary.MechanoidsSectionLabel".Translate(),
                    indentLevel,
                    OpenMask
                );

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
    }
}
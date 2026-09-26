using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace RaidSummary.Models
{
    public class MechClusterSummaryData : IExposable
    {
        private Faction mechFaction;
        private int tick;
        private Vector2 location;
        private int mechPawnCount = 0;
        private int buildingCount = 0;
        private Dictionary<PawnKindDef, int> mechCounts
            = new Dictionary<PawnKindDef, int>();
        private Dictionary<ThingDef, int> buildingCounts
            = new Dictionary<ThingDef, int>();
        public Faction MechFaction => mechFaction;
        public int MechCount => mechPawnCount;
        public int BuildingCount => buildingCount;

        public MechClusterSummaryData(){}
        public MechClusterSummaryData(Faction mechFaction, Map map, List<Thing> spawnedThings)
        {
            this.mechFaction = mechFaction;
            tick = Find.TickManager.TicksAbs;
            location = Find.WorldGrid.LongLatOf(map.Tile);

            foreach(Thing thing in spawnedThings)
            {
                if(thing is Pawn pawn && pawn.RaceProps.IsMechanoid)
                {
                    UpdateMechCount(pawn.kindDef);
                }
                else if (thing is Building building)
                {
                    UpdateBuildingCount(building.def);
                }
            }
        }

        private void UpdateMechCount(PawnKindDef mechKindDef)
        {
            if(!mechCounts.ContainsKey(mechKindDef))
                mechCounts[mechKindDef] = 0;

            mechCounts[mechKindDef]++;

            mechPawnCount++;
        }

        private void UpdateBuildingCount(ThingDef buildingDef)
        {
            if(!buildingCounts.ContainsKey(buildingDef))
                buildingCounts[buildingDef] = 0;

            buildingCounts[buildingDef]++;

            buildingCount++;
        }

        public Dictionary<PawnKindDef,int>.Enumerator MechEnumerator() => mechCounts.GetEnumerator();
        public Dictionary<ThingDef, int>.Enumerator BuildingEnumerator() => buildingCounts.GetEnumerator();
        public TaggedString GetFactionName(bool applyTag = false, bool capitalFirst = true)
        {
            string factionName = mechFaction.Name;
            if(capitalFirst) factionName = factionName.CapitalizeFirst();
            return applyTag ? factionName.ApplyTag(mechFaction) : (TaggedString) factionName;
        }
        public string GetMechClusterDate() => GenDate.DateFullStringWithHourAt(tick, location);
        public void ExposeData()
        {
            Scribe_References.Look(ref mechFaction, "mechFaction");
            Scribe_Values.Look(ref tick, "tick");
            Scribe_Values.Look(ref location, "location");
            Scribe_Values.Look(ref mechPawnCount, "mechPawnCount");
            Scribe_Values.Look(ref buildingCount, "buildingCount");
            Scribe_Collections.Look(ref mechCounts, "mechCounts", LookMode.Def, LookMode.Value);
            Scribe_Collections.Look(ref buildingCounts, "buildingCounts", LookMode.Def, LookMode.Value);
        }
    }
}
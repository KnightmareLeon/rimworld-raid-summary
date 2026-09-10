using UnityEngine;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace RaidSummary.UI
{
    public class ThingRootSummaryNode : TreeNode
    {
        private readonly Dictionary<ThingDef, ThingSummaryNode> childrenThingNodes
            = new Dictionary<ThingDef, ThingSummaryNode>();

        public void AddThingSummaryNode(ThingDef tDef, int openMask)
        {
            ThingSummaryNode newNode = new ThingSummaryNode(openMask);
            newNode.SetOpen(openMask, false);
            childrenThingNodes.Add(tDef, newNode);
        }

        public ThingSummaryNode GetThingSummaryNode(ThingDef tDef) => childrenThingNodes.TryGetValue(tDef);
    }
}
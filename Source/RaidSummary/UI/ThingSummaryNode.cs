using UnityEngine;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace RaidSummary.UI
{
    public class ThingSummaryNode : TreeNode
    {
        public TreeNode QualitiesNode {get; private set;} = new TreeNode();
        public TreeNode MaterialsNode {get; private set;} = new TreeNode();
        public ThingSummaryNode(int openMask)
        {
            QualitiesNode.SetOpen(openMask, true);
            MaterialsNode.SetOpen(openMask, true);
        }
    }
}
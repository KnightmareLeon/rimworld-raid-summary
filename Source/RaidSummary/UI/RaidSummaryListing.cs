using UnityEngine;
using Verse;

namespace RaidSummary.UI
{
    public class RaidSummaryListing : Listing_Tree
    {
        public bool DrawNode(TreeNode node, string label, int indentLevel, int openMask)
        {
            bool changed = OpenCloseWidget(node, indentLevel, openMask);

            LabelLeft(label, null, indentLevel);

            return changed;
        }

        public void DrawLabel(string label, int indentLevel)
        {
            LabelLeft(label, null, indentLevel);
            EndLine();
        }

        public void DrawSection(TreeNode node, string label, int indentLevel, int openMask)
        {
            OpenCloseWidget(node, indentLevel, openMask);

            LabelLeft(label, null, indentLevel);
            EndLine();
        }
    }
}
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

        public void DrawLabelForThing(ThingDef tDef, ref int indentLevel, string extraInfo = "")
        {
            if (tDef.uiIcon != null && tDef.uiIcon != BaseContent.BadTex)
                indentLevel++;
				Widgets.DefIcon(new Rect(XAtIndentLevel(indentLevel) - 6f, curY, 20f, 20f), tDef, null, 1f, null, drawPlaceholder: true);
            LabelLeft(tDef.LabelCap + extraInfo, null, indentLevel);
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
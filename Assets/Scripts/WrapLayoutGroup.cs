using UnityEngine;
using UnityEngine.UI;

public class WrapLayoutGroup : LayoutGroup
{
    public float spacingX = 10f;
    public float spacingY = 10f;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        LayoutChildren();
    }

    public override void CalculateLayoutInputVertical()
    {
        LayoutChildren();
    }

    public override void SetLayoutHorizontal() { }
    public override void SetLayoutVertical() { }

    private void LayoutChildren()
    {
        float parentWidth = rectTransform.rect.width;

        float x = padding.left;
        float y = padding.top;
        float rowHeight = 0f;

        foreach (RectTransform child in rectChildren)
        {
            float childWidth = LayoutUtility.GetPreferredWidth(child);
            float childHeight = LayoutUtility.GetPreferredHeight(child);

            // Wrap to next line
            if (x + childWidth > parentWidth - padding.right)
            {
                x = padding.left;
                y += rowHeight + spacingY;
                rowHeight = 0f;
            }

            SetChildAlongAxis(child, 0, x, childWidth);
            SetChildAlongAxis(child, 1, y, childHeight);

            x += childWidth + spacingX;
            rowHeight = Mathf.Max(rowHeight, childHeight);
        }
    }
}

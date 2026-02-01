using UnityEngine;

public class HoverText : Hoverable
{
    public static HoverText hovered;

    public string text;

    public override void SetHovered(bool isHovered)
    {
        base.SetHovered(isHovered);

        if (isHovered && hovered != this)
        {
            HoverBox.StartHovering(this);
            hovered = this;
        }
        if (!isHovered && hovered == this)
        {
            HoverBox.StopHovering(this);
            hovered = null;
        }
    }
}

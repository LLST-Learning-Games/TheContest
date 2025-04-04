using Godot;
using System;

public partial class Draggable : ColorRect
{
    public override Variant _GetDragData(Vector2 atPosition)
    {
        GD.Print("Draggable!");
        SetDragPreview(GetDragPreview());
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        return this;
    }

    public ColorRect GetDragPreview()
    {
        var colorRect = new ColorRect();
        colorRect.Color = this.Color;
        colorRect.Size = this.Size;
        return colorRect;
    }

    public override void _Notification(int what)
    {
        if (what == NotificationDragEnd)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }
}
 
using Godot;
using System;

public partial class DialogRoomButton : Button
{
    [Export] private PackedScene _dialogScene;
    [Export] private Control _parent;

    public void OnClick()
    {
        var screen = _dialogScene.Instantiate<Control>();
        _parent.AddChild(screen);
    }
}

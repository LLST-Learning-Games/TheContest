using Godot;
using System;
using Godot.Collections;

public partial class EscapeUi : ColorRect
{
    [Export] private Array<Control> _nodesToShowOnEscape;

    public override void _Ready()
    {
        ClearUi();
    }

    private void ClearUi()
    {
        foreach (Control node in _nodesToShowOnEscape)
        {
            node.SetVisible(false);
            node.MouseFilter = MouseFilterEnum.Pass;
            if (node is Button button)
            {
                button.Disabled = true;
            }
        }

        MouseFilter = MouseFilterEnum.Pass;
    }

    public void OnEscape()
    {
        foreach (Control node in _nodesToShowOnEscape)
        {
            node.SetVisible(true);
            node.MouseFilter = MouseFilterEnum.Stop;
            if (node is Button button)
            {
                button.Disabled = false;
            }
        }
        MouseFilter = MouseFilterEnum.Stop;
    }

    public void SetFade(float alpha)
    {
        var oldColour = Modulate; 
        var newColour = new Color(
            oldColour.R,
            oldColour.G,
            oldColour.B, 
            alpha);
        Modulate = newColour;
    }
    
    public void OnPressed()
    {
        ClearUi();
        var bootstrap = GetTree().Root.GetNode<Bootstrap>("Bootstrap");
        bootstrap.RestartGame();
    }
}

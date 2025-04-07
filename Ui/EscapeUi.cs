using Godot;
using System;
using Godot.Collections;
using Systems;
using Systems.Currency;

public partial class EscapeUi : ColorRect
{
    [Export] private Array<Control> _nodesToShowOnEscape;
    [Export] private Label _celebrationLabel;

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
        CelebrateWealth();
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
    
    private void CelebrateWealth()
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("newCash");
        _celebrationLabel.Text = $"You have contributed ${currency.Balance} to the hallowed treasury of your family.";
    }
}

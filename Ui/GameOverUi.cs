using Godot;
using Systems;
using Systems.Currency;

namespace TheContest.Ui;

public partial class GameOverUi : Button
{
    [Export] private Label _penaltyLabel;
    
    public override void _Ready()
    {
        Visible = false;
        MouseFilter = MouseFilterEnum.Pass;
        Disabled = true;
        var character = GetTree().GetFirstNodeInGroup("Player");
        var characterHealth = character.GetNode<HealthComponent>("HealthComponent");
        characterHealth.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        PayDeathPenalty();
        MouseFilter = MouseFilterEnum.Stop;
        Disabled = false;
        Visible = true;
    }

    public override void _Pressed()
    {
        var bootstrap = GetTree().Root.GetNode<Bootstrap>("Bootstrap");
        bootstrap.RestartGame(shouldPlayNarrative: true);
        base._Pressed();
    }

    private void PayDeathPenalty()
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("newCash");
        var penalty = currency.Balance / 2f;
        currency.OnCurrencyChanged = null;      // hack to stop UI from updating
        currency.UpdateCurrencyByDelta(-penalty);
        _penaltyLabel.Text = $"Your family has managed to scavenge ${currency.Balance} from your ignoble death.";
    }
}
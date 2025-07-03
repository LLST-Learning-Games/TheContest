using Godot;
using Systems;
using Systems.Currency;

public partial class PulseUpgradePanel : Control
{
    [Export] private float _energyUpgradeAmount = 10;
    [Export] private float _energyUpgradeCost = 10;
    [Export] private Label _energyLabel;
    
    [Export] private float _rechargeUpgradeAmount = 5;
    [Export] private float _rechargeUpgradeCost = 10;
    [Export] private Label _rechargeLabel;
    
    private ProjectileLibrary Library => _library ??= SystemLoader.GetSystem<ProjectileLibrary>();
    private ProjectileLibrary _library;
    
    private CurrencySystem CurrencySystem => _currencySystem ??= SystemLoader.GetSystem<CurrencySystem>();
    private CurrencySystem _currencySystem;

    public override void _Ready()
    {
        _energyLabel.Text = $"{Library.PlayerPulse.MaxEnergy}";
        _rechargeLabel.Text = $"{Library.PlayerPulse.RechargeRate}";
    }

    public void UpgradeMaxEnergy()
    {
        var currency = CurrencySystem.GetCurrency("cash");
        if (!currency.CanAfford(_energyUpgradeCost))
        {
            return;
        }
        currency.UpdateCurrencyByDelta(-_energyUpgradeCost);
        Library.PlayerPulse.UpdateMaxEnergyBy(_energyUpgradeAmount);
        _energyLabel.Text = $"{Library.PlayerPulse.MaxEnergy}";
    }
    
    public void UpgradeRechargeRate()
    {
        var currency = CurrencySystem.GetCurrency("cash");
        if (!currency.CanAfford(_rechargeUpgradeCost))
        {
            return;
        }
        currency.UpdateCurrencyByDelta(-_rechargeUpgradeCost);
        Library.PlayerPulse.UpdateRechargeRateBy(_rechargeUpgradeAmount);
        _rechargeLabel.Text = $"{Library.PlayerPulse.RechargeRate}";
    }
}

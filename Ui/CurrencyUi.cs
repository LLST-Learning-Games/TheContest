using Godot;
using System;
using Systems;
using Systems.Currency;

public partial class CurrencyUi : Label
{
    [Export] private string _id = "cash"; 
    private Currency _currency;
    
    public override void _Ready()
    {
        if (SystemLoader.IsSystemLoadComplete)
        {
            Initialize();
        }
        else
        {
            SystemLoader.OnSystemLoadComplete += Initialize;
        }
        UpdateUi(_currency.Balance);
    }

    private void Initialize()
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        _currency = currencySystem.GetCurrency(_id);
        _currency.OnCurrencyChanged += UpdateUi;
    }

    private void UpdateUi(float value)
    {
        Text = "$" + value;
    }

    public override void _ExitTree()
    {
        _currency.OnCurrencyChanged -= UpdateUi;
        base._ExitTree();
    }
}

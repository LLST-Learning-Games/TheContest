using Godot;
using System;
using Systems;
using Systems.Currency;

public partial class CurrencyUi : Label
{
    public override void _Ready()
    {
        SystemLoader.OnSystemLoadComplete += Initialize;
        UpdateUi(0);
    }

    private void Initialize()
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var cash = currencySystem.GetCurrency("cash");
        cash.OnCurrencyChanged += UpdateUi;
    }

    private void UpdateUi(float value)
    {
        Text = "$" + value;
    }
}

using System;
using Godot;

namespace Systems.Currency;

public partial class Currency : Node
{
    public CurrencyDefinition Definition { get; internal set; }
    public float Balance { get; internal set; }

    public Action<float> OnCurrencyChanged;

    public void Initialize(CurrencyDefinition definition)
    {
        Definition = definition;
        Balance = definition.StartingBalance;
    }
    
    public bool CanAfford(float amount) => amount >= Balance;
    
    public void UpdateCurrencyByDelta(float delta)
    {
        SetCurrency(Balance + delta);    
    }
    
    public void SetCurrency(float newValue)
    {
        Balance = newValue;
        OnCurrencyChanged?.Invoke(Balance);
    } 
}
using Godot;
using System;
using Systems;
using Systems.Currency;

public partial class Character : CharacterBody2D
{
    [Export] private HealthComponent _healthComponent;
    
    public override void _Ready()
    {
        _healthComponent.OnDeath += OnDeath;
    }
    
    private void OnDeath()
    {
        QueueFree();
    }

    private void NerfCurrencyOnDeath()
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("cash");
        currency.SetCurrency(currency.Balance / 2f);
    }
    
}

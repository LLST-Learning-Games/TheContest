using Godot;
using System;
using Systems;
using Systems.Currency;

public partial class Character : CharacterBody2D
{
    [Export] private Camera2D _camera;
    [Export] private HealthComponent _healthComponent;
    
    public override void _Ready()
    {
        _healthComponent.OnDeath += OnDeath;
    }
    
    private void OnDeath()
    {
        _camera.PositionSmoothingEnabled = false;
        _camera.Reparent(GetTree().Root);
        QueueFree();
    }

    private void NerfCurrencyOnDeath()
    {
        var currencySystem = SystemLoader.GetSystem<CurrencySystem>();
        var currency = currencySystem.GetCurrency("cash");
        currency.SetCurrency(currency.Balance / 2f);
    }
    
}

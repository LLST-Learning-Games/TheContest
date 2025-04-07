using Godot;
using Godot.Collections;

namespace Systems.Currency;

public partial class CurrencySystem : BaseSystem
{
    [Export] private Array<CurrencyDefinition> _currencyBootstrap;

    private static Dictionary<string, Currency> _currencies;
    
    public override void Initialize()
    {
        _currencies = new Dictionary<string, Currency>();
        foreach (var definition in _currencyBootstrap)
        {
            var currency = new Currency();
            currency.Initialize(definition);
            AddChild(currency);
            _currencies.Add(definition.Id, currency);
        }
    }

    public Currency GetCurrency(string id)
    {
        if(_currencies.TryGetValue(id, out var currency))
        {
            return currency;
        }
        
        GD.PrintErr("[CurrencySystem] Attempting to look up currency {" + id + "} but it doesn't exist.");
        return null;
    }
    
    public override void OnGameplayEnd()
    {
        MoveNewCashToCash();
        foreach (var currency in _currencies.Values)
        {
            currency.OnGameplayEnd();
        }
    }

    public void MoveNewCashToCash()
    {
        var newCurrency = GetCurrency("newCash");
        if (newCurrency == null)
        {
            return;
        }
        var currency = GetCurrency("cash");
        currency.UpdateCurrencyByDelta(newCurrency.Balance);
        newCurrency.Balance = 0;
    }
}
using Godot;

namespace Systems.Currency;

public partial class CurrencyDefinition : Resource
{
    [Export] public string Id = "cash";
    [Export] public float StartingBalance = 0; 
}
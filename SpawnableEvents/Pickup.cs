using Godot;
using Systems;
using Systems.Currency;

namespace TheContest.Projectiles.SpawnableEvents;

public partial class Pickup : Node2D
{
    [Export] private float _rewardAmount = 10f;
    [Export] private string _rewardId = "cash";
    [Export] private Area2D _area2D;

    public override void _Ready()
    {
        _area2D.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Character)
        {
            var wallet = SystemLoader.GetSystem<CurrencySystem>();
            var currency = wallet.GetCurrency(_rewardId);
            currency.UpdateCurrencyByDelta(_rewardAmount);
            QueueFree();
        }
    }
}
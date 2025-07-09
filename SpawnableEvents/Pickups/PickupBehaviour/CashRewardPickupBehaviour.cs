using Godot;
using Systems;
using Systems.Currency;

namespace TheContest.Projectiles.SpawnableEvents;

public partial class CashRewardPickupBehaviour : PickupBehaviour
{
    [Export] private float _rewardAmount = 10f;
    [Export] private string _rewardId = "cash";
    internal override void PickItUp()
    {
        var wallet = SystemLoader.GetSystem<CurrencySystem>();
        var currency = wallet.GetCurrency(_rewardId);
        currency.UpdateCurrencyByDelta(_rewardAmount);
    }

    internal override string SetText()
    {
        return "$" + _rewardAmount.ToString();
    }
}
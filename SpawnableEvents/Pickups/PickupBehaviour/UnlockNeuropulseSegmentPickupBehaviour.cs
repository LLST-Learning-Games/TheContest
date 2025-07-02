using Godot;
using Systems;
using Systems.Currency;

namespace TheContest.Projectiles.SpawnableEvents;

public partial class UnlockNeuropulseSegmentPickupBehaviour : PickupBehaviour
{
    RandomNumberGenerator rng = new RandomNumberGenerator();
    
    private ProjectileLibrary Library => _library ??= SystemLoader.GetSystem<ProjectileLibrary>();
    private ProjectileLibrary _library;
    
    public UnlockNeuropulseSegmentPickupBehaviour()
    {
        rng.Randomize();
    }
    
    internal override void PickItUp()
    {
        var library = SystemLoader.GetSystem<ProjectileLibrary>();
        var ids = library.GetAllUnlockableIds();
        int index = rng.RandiRange(0, ids.Count - 1);
        var id = ids[index];
        
        Library.UnlockPulseId(id);
        GD.Print($"[{GetType().Name}] Unlocked neuropulse segment: {id}");
    }
}
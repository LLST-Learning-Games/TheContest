using Godot;

namespace TheContest.Projectiles.SpawnableEvents;

public abstract partial class PickupBehaviour : Resource
{
    internal abstract void PickItUp();

    internal virtual string SetText()
    {
        return "";
    }
}
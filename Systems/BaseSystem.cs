using Godot;

namespace Systems;

public abstract partial class BaseSystem : Node
{
    [Export] internal string _id;
    
    public abstract void Initialize();
    public virtual void OnGameplayEnd()  { /*..*/ }
    public virtual void OnGameplayStart()  { /*..*/ }
}
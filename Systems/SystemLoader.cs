using Godot;
using Godot.Collections;

namespace Systems;

public partial class SystemLoader : Node
{
    [Export] private Array<BaseSystem> _systemBootstrap;

    private static Dictionary<string, BaseSystem> _initializedSystems;

    public override void _Ready()
    {
        _initializedSystems = new Dictionary<string, BaseSystem>();
        foreach (var system in _systemBootstrap)
        {
            system.Initialize();
            _initializedSystems.Add(system.Name, system);
        }
    }

    public static BaseSystem GetSystem(string id)
    {
        if(_initializedSystems.TryGetValue(id, out var system))
        {
            return system;
        }
        
        GD.PrintErr("[SystemLoader] Attempting to look up system {" + id + "} but it doesn't exist.");
        return null;
    }

}

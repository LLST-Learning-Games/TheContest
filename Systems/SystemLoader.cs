using System;
using Godot;
using Godot.Collections;

namespace Systems;

public partial class SystemLoader : Node
{
    [Export] private Array<BaseSystem> _systemBootstrap;

    public static bool IsSystemLoadComplete { get; private set; }
    public static Action OnSystemLoadComplete;
    
    private static Dictionary<string, BaseSystem> _initializedSystems;

    public override void _Ready()
    {
        _initializedSystems = new Dictionary<string, BaseSystem>();
        foreach (var system in _systemBootstrap)
        {
            system.Initialize();
            _initializedSystems.Add(system.GetType().Name, system);
        }
        OnSystemLoadComplete?.Invoke();
        IsSystemLoadComplete = true;
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
    
    public static T GetSystem<T>() where T : BaseSystem
    {
        return GetSystem(typeof(T).Name) as T;
    }

    public static void OnGameplayEnd()
    {
        foreach (var system in _initializedSystems.Values)
        {
            system.OnGameplayEnd();
        }
    }

}

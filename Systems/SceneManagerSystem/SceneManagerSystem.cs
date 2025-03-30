using Godot;
using Godot.Collections;

namespace Systems.SceneManager;

public partial class SceneManagerSystem : BaseSystem
{
    [Export] private Dictionary<string, PackedScene> _scenePrefabs; 
    public override void Initialize()
    {
        // ..
    }
    
    public PackedScene GetScenePrefab(string sceneName) => _scenePrefabs[sceneName];
}
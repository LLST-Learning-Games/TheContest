using Godot;
using System;
using Systems;
using Systems.SceneManager;

public partial class MainMenu : Control
{
    [Export] private Control _familyHouse;
    [Export] private Control _narrativeParent;
    private SceneManagerSystem SceneManager => _sceneManager ?? SystemLoader.GetSystem<SceneManagerSystem>();
    private SceneManagerSystem _sceneManager;
    public void OnStartNewGame()
    {
        var onAwakeNarrativeScene = SceneManager.GetScenePrefab("OnAwakeNarrative");
        var onAwakeNarrativeInstantiated = onAwakeNarrativeScene.Instantiate<Control>();
        _narrativeParent.AddChild(onAwakeNarrativeInstantiated);
        _familyHouse.Show();
    }
    
    public void ShowFamilyHouse() => _familyHouse.Show();
}

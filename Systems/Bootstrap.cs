using System;
using Godot;
using Systems;
using Systems.SceneManager;

public partial class Bootstrap : Node2D
{
    [Export] private CanvasLayer _canvas;
    
    private Control _mainScreenUi;
    private Node _gameplaySceneInstantiated;
    
    private SceneManagerSystem SceneManager => _sceneManager ?? SystemLoader.GetSystem<SceneManagerSystem>();
    private SceneManagerSystem _sceneManager;

    public Action OnGameplayEnd;

    public override void _Ready()
    { 
        InstantiateMainMenu();
    }

    private void InstantiateMainMenu()
    {
        var mainMenu = SceneManager.GetScenePrefab("MainMenu");
        _mainScreenUi = mainMenu.Instantiate<Control>();
        _canvas.AddChild(_mainScreenUi);
        ConnectStartButton();
        ConnectCustomizeButton();
    }

    private void ConnectStartButton()
    {
        var startButtonNode = GetTree().GetFirstNodeInGroup("StartButton");
        var startButton = startButtonNode as Button;
        startButton.Pressed += OnStartButton;
    }
    
    private void ConnectCustomizeButton()
    {
        var customizeButtonNode = GetTree().GetFirstNodeInGroup("CustomizeButton");
        var customizeButton = customizeButtonNode as Button;
        customizeButton.Pressed += OnCustomizeWeaponSelect;
    }

    private void OnStartButton()
    {
        var gameplayScene = SceneManager.GetScenePrefab("Gameplay");
        _gameplaySceneInstantiated = gameplayScene.Instantiate();
        AddChild(_gameplaySceneInstantiated);
        _mainScreenUi.QueueFree();
    }

    public void RestartGame()
    {
        OnGameplayEnd?.Invoke();
        _gameplaySceneInstantiated.QueueFree();
        InstantiateMainMenu();
    }

    private void OnCustomizeWeaponSelect()
    {
        var customizationScene = SceneManager.GetScenePrefab("NeuroPulseCustomizer");
        var instantiated = customizationScene.Instantiate();
        _canvas.AddChild(instantiated);
    }
}

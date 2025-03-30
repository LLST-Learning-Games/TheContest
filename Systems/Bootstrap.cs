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

    public override void _Ready()
    { 
        InstantiateMainMenu();
        //SystemLoader.OnSystemLoadComplete += InstantiateMainMenu;
    }

    private void InstantiateMainMenu()
    {
        var mainMenu = SceneManager.GetScenePrefab("MainMenu");
        _mainScreenUi = mainMenu.Instantiate<Control>();
        _canvas.AddChild(_mainScreenUi);
        var startButtonNode = GetTree().GetFirstNodeInGroup("StartButton");
        GD.Print($"Found a startButton Node with the name: {startButtonNode.Name}");
        var startButton = startButtonNode as Button;
        startButton.Pressed += OnStartButton;
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
        _gameplaySceneInstantiated.QueueFree();
        InstantiateMainMenu();
    }
}

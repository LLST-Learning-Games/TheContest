using Godot;

public partial class Bootstrap : Node2D
{
    [Export] private CanvasLayer _canvas;
    [Export] private PackedScene _gameplayScene;
    [Export] private PackedScene _mainMenu;
    
    private Control _mainScreenUi;
    private Node _gameplaySceneInstantiated;

    public override void _Ready()
    {
        InstantiateMainMenu();
    }

    private void InstantiateMainMenu()
    {
        _mainScreenUi = _mainMenu.Instantiate<Control>();
        _canvas.AddChild(_mainScreenUi);
        var startButtonNode = GetTree().GetFirstNodeInGroup("StartButton");
        GD.Print($"Found a startButton Node with the name: {startButtonNode.Name}");
        var startButton = startButtonNode as Button;
        startButton.Pressed += OnStartButton;
    }

    private void OnStartButton()
    {
        _gameplaySceneInstantiated = _gameplayScene.Instantiate();
        AddChild(_gameplaySceneInstantiated);
        _mainScreenUi.QueueFree();
    }

    public void RestartGame()
    {
        _gameplaySceneInstantiated.QueueFree();
        InstantiateMainMenu();
    }
}

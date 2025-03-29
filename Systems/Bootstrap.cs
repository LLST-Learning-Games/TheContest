using Godot;

public partial class Bootstrap : Node2D
{
    [Export] private PackedScene _gameplayScene;
    [Export] private Control _mainScreenUi;

    private void OnStartButton()
    {
        var gameplayScene = _gameplayScene.Instantiate();
        _mainScreenUi.Visible = false;
        AddChild(gameplayScene);
    }
}

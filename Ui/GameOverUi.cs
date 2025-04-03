using Godot;

namespace TheContest.Ui;

public partial class GameOverUi : Button
{
    public override void _Ready()
    {
        Visible = false;
        MouseFilter = MouseFilterEnum.Pass;
        Disabled = true;
        var character = GetTree().GetFirstNodeInGroup("Player");
        var characterHealth = character.GetNode<HealthComponent>("HealthComponent");
        characterHealth.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        MouseFilter = MouseFilterEnum.Stop;
        Disabled = false;
        Visible = true;
    }

    public override void _Pressed()
    {
        var bootstrap = GetTree().Root.GetNode<Bootstrap>("Bootstrap");
        bootstrap.RestartGame();
        base._Pressed();
    }
}
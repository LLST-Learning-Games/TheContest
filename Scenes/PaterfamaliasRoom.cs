using Godot;
using System;
using System.Threading.Tasks;

public partial class PaterfamaliasRoom : Control
{
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private NarrativeLabel _narrativeLabel_Narrator;
    [Export] private NarrativeLabel _narrativeLabel_Father;
    [Export] private Button _communeWithPaterButton;
    [Export] private Button _advanceConversationButton;
    [Export] private ColorRect _screenFade;

    
    public override void _Ready()
    {
        base._Ready();
        _animationPlayer.Play("OnPaterEnter");
        _animationPlayer.AnimationFinished += OnFatherEnterAnimationComplete;
        HideButton(_communeWithPaterButton);
        HideButton(_advanceConversationButton);
    }

    private void HideButton(Button button)
    {
        var buttonModulate = button.Modulate;
        buttonModulate.A = 0;
        button.Modulate = buttonModulate;
    }

    private async void OnFatherEnterAnimationComplete(StringName _)
    {
        await _narrativeLabel_Narrator.InitializeNarrative();
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(_communeWithPaterButton, "modulate:a", 1, 0.5);
        _animationPlayer.AnimationFinished -= OnFatherEnterAnimationComplete;
    }

    private async void OnStartClick()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(_communeWithPaterButton, "modulate:a", 0, 0.1);
        
        await _narrativeLabel_Narrator.AdvanceNarrative();
        await Task.Delay(1000);
        
        _animationPlayer.Play("OnPaterGlow");
        _animationPlayer.AnimationFinished += OnFatherGlowAnimationComplete;
    }

    private async void OnFatherGlowAnimationComplete(StringName animName)
    {
        await _narrativeLabel_Narrator.AdvanceNarrative();
        await Task.Delay(2000);
        _narrativeLabel_Narrator.ResetNarrative();
        await Task.Delay(1000);
        await _narrativeLabel_Father.InitializeNarrative();
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(_advanceConversationButton, "modulate:a", 1, 0.5);
        _animationPlayer.AnimationFinished -= OnFatherGlowAnimationComplete;
    }

    public async void OnAdvanceConversationClick()
    {
        await _narrativeLabel_Father.AdvanceNarrative();
    }

    public async void OnLeaveClick()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(_screenFade, "modulate:a", 1, 1);
        await ToSignal(tween, "finished");
        QueueFree();
    }
}

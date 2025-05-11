using Godot;
using System;
using System.Threading.Tasks;

public partial class PaterfamaliasRoom : Control
{
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private NarrativeLabel _narrativeLabel_Narrator;
    [Export] private NarrativeLabel _narrativeLabel_Father;
    [Export] private Button _communeWithPaterButton;
    
    public override void _Ready()
    {
        base._Ready();
        _animationPlayer.Play("OnPaterEnter");
        _animationPlayer.AnimationFinished += OnFatherEnterAnimationComplete;
        var buttonModulate = _communeWithPaterButton.Modulate;
        buttonModulate.A = 0;
        _communeWithPaterButton.Modulate = buttonModulate;
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
        await Task.Delay(1000);
        _narrativeLabel_Narrator.ResetNarrative();
        _narrativeLabel_Father.InitializeNarrative();
        _animationPlayer.AnimationFinished -= OnFatherGlowAnimationComplete;
    }
}

using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public partial class NarrativeLabel : Label
{
    private const double FADE_DURATION = 0.5;
    [Export] private Array<string> _narrativeSteps;

    private int _currentStep = 0;

    public override void _Ready()
    {
        Text = "";
        var textModulate = Modulate;
        textModulate.A = 0;
        Modulate = textModulate;
    }

    public async Task InitializeNarrative()
    {
        _currentStep = 0;
        Text = _narrativeSteps[_currentStep];
        await FadeToAlpha(1);
        
    }
    public async Task AdvanceNarrative()
    {
        await FadeToAlpha(0);
        ++_currentStep;
        if (_currentStep >= _narrativeSteps.Count)
        {
            ResetNarrative();
            return;
        }
        Text = _narrativeSteps[_currentStep];
        await FadeToAlpha(1);
    }

    private async Task FadeToAlpha(float alpha)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate:a", alpha, FADE_DURATION);
        await ToSignal(tween, "finished");
    }

    public void ResetNarrative() 
    {
        FadeToAlpha(0);
        _currentStep = 0;
    }
}

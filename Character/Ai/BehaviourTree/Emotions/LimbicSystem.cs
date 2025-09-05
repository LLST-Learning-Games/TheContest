using Godot;
using Godot.Collections;

namespace Behaviours.Emotions;

public partial class LimbicSystem : Node
{
    [Export] private Array<Emotion> _emotionBootstrap;
    private Dictionary<string, Emotion> _emotions;

    public override void _Ready()
    {
        foreach (Emotion emotion in _emotionBootstrap)
        {
            _emotions.Add(emotion.Id, emotion);
        }
    }

    public Emotion GetEmotion(string emotionId)
    {
        return _emotions[emotionId];
    }

    public Emotion TryUpdateEmotion(string emotionId, int delta)
    {
        if (_emotions.TryGetValue(emotionId, out var emotion))
        {
            emotion.UpdateIntensity(delta);
            return emotion;
        }

        return null;
    }

}
using Godot;

namespace Behaviours;

public partial class BehaviourCheckEmotion :BehaviourBase
{
    [Export] private string _emotionId = "fear";
    public override BehaviourState UpdateNode(double delta, BehaviourTreeBlackboard blackboard)
    {
        var emotion = blackboard.LimbicSystem.GetEmotion(_emotionId);
        if (emotion is null)
        {
            if(blackboard.IsVerbose)
            {
                GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] No emotion with id {_emotionId} found!");
            }
            return BehaviourState.Failure;
        }

        if (!emotion.IsTriggered)
        {
            if(blackboard.IsVerbose)
            {
                GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] Found emotion {_emotionId}, but it is not triggered.");
            }
            return BehaviourState.Failure;
        }
        
        if(blackboard.IsVerbose)
        {
            GD.Print($"[{GetType().Name}] [{blackboard.Actor.Name}] Found emotion {_emotionId}, and it is triggered. Activating response!");
        }
        
        return BehaviourState.Success;
    }

    public override void ResetBehaviour(BehaviourTreeBlackboard blackboard)
    {
        // ..
    }
}
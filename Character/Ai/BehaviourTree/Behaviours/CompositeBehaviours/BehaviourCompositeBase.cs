using Godot;
using Godot.Collections;

namespace Behaviours
{

    public abstract partial class BehaviourCompositeBase : BehaviourBase
    {
        [Export] protected Array<BehaviourBase> _children;
    }
}
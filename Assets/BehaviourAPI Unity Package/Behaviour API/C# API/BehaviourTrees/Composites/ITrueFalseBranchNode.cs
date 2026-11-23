using System;

namespace BehaviourAPI.BehaviourTrees
{
    public interface ITrueFalseBranchNode
    {
        TrueFalseBranchNode SetNodeIndexFunction(Func<bool> nodeIndexFunction);
    }
}
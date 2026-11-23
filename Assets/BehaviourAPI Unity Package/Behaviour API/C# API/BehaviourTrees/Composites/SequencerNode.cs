namespace BehaviourAPI.BehaviourTrees
{
    using Core;

    /// <summary>
    /// Serial Composite node that executes its children until one of them returns Failure.
    /// </summary>
    [System.Serializable]
    public class SequencerNode : SerialCompositeNode
    {
        protected override bool KeepExecutingNextChild(Status status)
        {
            return status == Status.Success;
        }

        protected override Status GetFinalStatus(Status status)
        {
            return status;
        }

        public override string className()
        {
            return "-->";
        }
    }
}
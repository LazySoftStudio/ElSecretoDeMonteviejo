namespace BehaviourAPI.BehaviourTrees
{
    using Core;
    using UnityEngine;

    public abstract class ReactiveBranchNode : CompositeNode
    {
        int currentNode = -1;
        bool lastframeUpdated = false;
        protected BTNode m_SelectedNode;
        public int waitTimeFrames;
        bool wait;
      public  bool syncUpdate = false;
        int randomDelay;
        protected abstract int SelectBranchIndex();

        public override void OnStarted()
        {
            base.OnStarted();
            wait = waitTimeFrames > 0;
            randomDelay = syncUpdate ? 0 : Random.Range(0,10);
            lastframeUpdated = false;
            int branchIndex = SelectBranchIndex();
            if (branchIndex < 0) branchIndex = 0;
            if (branchIndex >= ChildCount) branchIndex = ChildCount - 1;
            m_SelectedNode = GetBTChildAt(branchIndex);
            currentNode = branchIndex;

            m_SelectedNode?.OnStarted();
            
        }
        protected override Status UpdateStatus()
        {
            if (!wait || (Time.frameCount + randomDelay) % waitTimeFrames == 0)
            {
                int branchIndex = SelectBranchIndex();
                if (currentNode != branchIndex)
                {
                    m_SelectedNode?.OnStopped();
                    if (branchIndex < 0) branchIndex = 0;
                    if (branchIndex >= ChildCount) branchIndex = ChildCount - 1;
                    currentNode = branchIndex;
                    m_SelectedNode = GetBTChildAt(branchIndex);
                    if (m_SelectedNode.Status == Status.None)
                        m_SelectedNode?.OnStarted();

                }
            }
            m_SelectedNode.OnUpdated();
            lastframeUpdated = true;
            return m_SelectedNode?.Status ?? Status.Failure;
            //return Status.Running;  
        }
        public override void OnPaused()
        {
            base.OnPaused();
            if (lastframeUpdated)
            {
                m_SelectedNode?.OnPaused();
            }
        }
        public override void OnStopped()
        {
            base.OnStopped();
            if (lastframeUpdated)
            {
                m_SelectedNode?.OnStopped();
            }
        }
        public override void OnUnpaused()
        {
            base.OnUnpaused();
            if (lastframeUpdated)
            {
                m_SelectedNode?.OnUnpaused();
            }
        }

    }
}

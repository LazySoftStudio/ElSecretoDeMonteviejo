using System;

namespace BehaviourAPI.BehaviourTrees
{
    public class TestReactiveBranchNode : ReactiveBranchNode
    {
        static Random Random = new Random();

        /// <summary>
        /// <inheritdoc/>
        /// Gets a random child node.
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected override int SelectBranchIndex()
        {
            var id = Random.Next(0, m_children.Count);
            return id;
        }
    }
   
}

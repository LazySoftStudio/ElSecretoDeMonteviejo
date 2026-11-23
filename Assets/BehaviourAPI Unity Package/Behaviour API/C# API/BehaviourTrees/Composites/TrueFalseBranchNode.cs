using System;

namespace BehaviourAPI.BehaviourTrees
{
    using Core;

    using Core.Perceptions;
    public class TrueFalseBranchNode : ReactiveBranchNode
    {
        /// <summary>
        /// The function used to get the branch index. The result will be clamped between 0 and child count.
        /// </summary>
        public Perception Perception;

        /// <summary>
        /// Set the function used to get the branch index.
        /// </summary>
        /// <param name="nodeIndexFunction">The value of the function.</param>
        /// <returns>The <see cref="FunctionBranchNode"/> itself.</returns>
     

        protected override int SelectBranchIndex()
        {
            bool index = Perception.Check();
            return index ? 1 : 0;
        }
    }
   
}

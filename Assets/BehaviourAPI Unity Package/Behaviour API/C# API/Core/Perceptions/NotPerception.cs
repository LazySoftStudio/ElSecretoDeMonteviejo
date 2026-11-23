using System.Collections.Generic;

namespace BehaviourAPI.Core.Perceptions
{
    [System.Serializable]
    public class NotPerception : CompoundPerception
    {
      
        /// <summary>
        /// Create a new or perception.
        /// </summary>
        public NotPerception() : base() { }

        /// <summary>
        /// Create a new or perception.
        /// </summary>
        /// <param name="perceptions">The list of subperceptions.</param>
        public NotPerception(List<Perception> perceptions) : base(perceptions) { }

        /// <summary>
        /// Create a new or perception.
        /// </summary>
        /// <param name="perceptions">The list of subperceptions.</param>
        public NotPerception(params Perception[] perceptions) : base(perceptions) { }

        public override bool allowMultiple()
        {
            return false;
        }

        /// <summary>
        /// Check all the sub perceptions and return true if any of them returned true.
        /// Returns false if the sub perception list is empty.
        /// </summary>
        /// <returns>true if any of the sub perceptions returned true and the sub perception list is not empty, false otherwise.</returns>
        public override bool Check()
        {
            if (Perceptions.Count == 0) return false;
          return  !Perceptions[0].Check();
        }
    }
}

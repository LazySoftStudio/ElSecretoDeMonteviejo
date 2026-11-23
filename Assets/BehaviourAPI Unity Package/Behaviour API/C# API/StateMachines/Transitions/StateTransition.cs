using System;
using System.Collections.Generic;

namespace BehaviourAPI.StateMachines
{
    using Core;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Transition between two states.
    /// </summary>
    [System.Serializable]
    public class StateTransition : Transition
    {
        #region ------------------------------------------ Properties -----------------------------------------

        public override int MaxOutputConnections => 1;

        #endregion

        #region -------------------------------------- Private variables -------------------------------------

        State _targetState;

        #endregion

        #region ---------------------------------------- Build methods ---------------------------------------

        /// <summary>
        /// Set the target state of the transition
        /// </summary>
        /// <param name="target">The target state.</param>
        protected internal void SetTargetState(State target) => _targetState = target;

        protected override void BuildConnections(List<Node> parents, List<Node> children)
        {
            base.BuildConnections(parents, children);

            if (children.Count > 0 && children[0] is State to)
                _targetState = to;
            else
                throw new ArgumentException();
        }
    public  StateTransition()
        {
         StatusFlags = (StatusFlags)-1;
         //StatusFlags = StatusFlags.Failure | StatusFlags.Finished | StatusFlags.Running | StatusFlags.Paused | StatusFlags.Unfinished | StatusFlags.Active | StatusFlags.Success;
        }
        #endregion

        #region --------------------------------------- Runtime methods --------------------------------------

        /// <summary>
        /// <inheritdoc/>
        /// Changes the current state of the fsm to the target state.
        /// </summary>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="MissingChildException">If target state is null</exception>
        public override bool Perform()
        {
            bool canBePerformed = base.Perform();
            if (canBePerformed)
            {
                if (_targetState == null) throw new MissingChildException(this, "The target state can't be null.");
                _fsm.SetCurrentState(_targetState, this);
            }
            return canBePerformed;
        }
        public override string className()
        {
            return "Transition";
        }

        #endregion
    }
}

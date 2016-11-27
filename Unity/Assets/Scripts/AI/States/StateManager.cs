using System;
using System.Collections.Generic;
using AI.States.Exceptions;

namespace AI.States
{
    public class StateManager
    {
        private int _currentStateId;

        private readonly Dictionary<int, IState> _stateTable;

        private readonly Dictionary<int, List<int>> _transitionTable;

        public StateManager()
        {
            _stateTable = new Dictionary<int, IState>();
            _transitionTable = new Dictionary<int, List<int>>();
        }

        public IState GetCurrentState()
        {
            return _stateTable[_currentStateId];
        }

        public IState GetState(int stateId)
        {
            if (!_stateTable.ContainsKey(stateId))
            {
                throw new StateDoesNotExistException("This state does not exist in the state table!");
            }
            return _stateTable[stateId];
        }

        public void SetCurrentState(int stateId)
        {
            if (!_stateTable.ContainsKey(stateId))
            {
                throw new StateDoesNotExistException("You set the current state to one that does not exist in the transition table!");
            }
            _currentStateId = stateId;
        }

        public void AddState(int stateId, IState state)
        {
            _stateTable.Add(stateId, state);
            _transitionTable[stateId] = new List<int>();
        }

        public void AddTransition(int initialStateId, int stateIdToTransitionTo )
        {
            _transitionTable[initialStateId].Add(stateIdToTransitionTo);
        }

        public void ChangeToState(int stateId)
        {
            if (_transitionTable[_currentStateId].Contains(stateId))
            {
                _currentStateId = stateId;
                _stateTable[_currentStateId].OnEnter();
            }
            else
            {
                throw new InvalidStateTransitionException("You cannot transition to this state! (StateID : " + stateId + " )");
            }
        }
    }
}
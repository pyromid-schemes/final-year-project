using System.Collections.Generic;
using AI.Pathfinding;
using AI.States;
using AI.States.PathfindingTest;

namespace AI.MobControllers
{
    class PathfindingTestController : SimpleMobController
    {

        // Use this for initialization
        public override List<PathfindingNode> GetOccupiedSpaces()
        {
            return null;
        }

        protected override void InitialiseStates()
        {
            StateManager = new StateManager();
            StateManager.AddState(1, new MoveState(this));
            StateManager.SetCurrentState(1);
        }
    }
}

using AI.Pathfinding;
using AI.States;
using AI.States.PathfindingTest;

namespace AI.MobControllers
{
    class PathfindingTestController : SimpleMobController
    {

        public Grid Grid;

        // Use this for initialization
        protected override void InitialiseStates()
        {
            StateManager = new StateManager();
            StateManager.AddState(1, new MoveState(this));
            StateManager.SetCurrentState(1);
        }
    }
}

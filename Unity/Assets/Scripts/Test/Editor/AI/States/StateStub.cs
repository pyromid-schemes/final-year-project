using System;
using AI.States;
/**
 * @author Daniel Burnley
 */
namespace Test.AI.States
{
    class StateStub : IState
    {
        public bool HasEntered;

        public void OnEnter()
        {
            HasEntered = true;
        }

        public void OnUpdate()
        {
            throw new NotImplementedException();
        }

        public void OnFixedUpdate()
        {
            throw new NotImplementedException();
        }
    }
}

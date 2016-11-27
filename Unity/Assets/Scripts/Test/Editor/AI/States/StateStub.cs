using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AI.States;

namespace Test.AI.States
{
    class StateStub : IState
    {
        public bool HasEntered = false;

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

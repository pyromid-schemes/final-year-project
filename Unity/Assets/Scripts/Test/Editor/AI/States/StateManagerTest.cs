using AI.States;
using AI.States.Exceptions;
using NUnit.Framework;

namespace Test.AI.States
{
    class StateManagerTest
    {
        private StateManager _stateManager;

        private enum States
        {
            Meow,
            Woof,
            Quack
        }

        [SetUp]
        public void SetUp()
        {
            _stateManager = new StateManager();
        }

        [Test]
        [ExpectedException(typeof(StateDoesNotExistException))]
        public void givenNoStates_whenTryingToGetState_throwException()
        {
            _stateManager.GetState((int)States.Meow);
        }

        [Test]
        [ExpectedException(typeof (StateDoesNotExistException)) ]
        public void givenOneState_whenTryingToSetCurrentStateToNonExistingState_throwException()
        {
            _stateManager.AddState((int) States.Meow, new StateStub());
            _stateManager.SetCurrentState((int) States.Woof);
        }

        [Test]
        public void givenState_whenAddingToStateManager_addStateCorrectly()
        {
            IState stateToAdd = new StateStub();
            _stateManager.AddState((int) States.Meow, stateToAdd);

            Assert.AreEqual(stateToAdd, _stateManager.GetState((int) States.Meow));
        }

        [Test]
        public void givenState_whenSettingToCurrentState_setCurrentStateCorrectly()
        {
            IState stateToAdd = new StateStub();
            _stateManager.AddState((int) States.Meow, stateToAdd);
            _stateManager.SetCurrentState((int) States.Meow);

            Assert.AreEqual(stateToAdd, _stateManager.GetCurrentState());
        }

        [Test]
        [ExpectedException(typeof(InvalidStateTransitionException))]
        public void givenTwoStatesThatHaveNoTransition_whenChangingToState_throwException()
        {
            _stateManager.AddState((int) States.Meow, new StateStub());
            _stateManager.AddState((int) States.Woof, new StateStub());
            _stateManager.ChangeToState((int) States.Woof);
        }

        [Test]
        public void givenTwoStatesThatHaveATransition_whenChangingToState_updateCurrentStateCorrectly()
        {
            IState stateOne = new StateStub();
            IState stateTwo = new StateStub();

            _stateManager.AddState((int) States.Meow, stateOne);
            _stateManager.AddState((int) States.Woof, stateTwo);
            _stateManager.AddTransition((int) States.Meow, (int) States.Woof);
            _stateManager.SetCurrentState((int) States.Meow);
            _stateManager.ChangeToState((int) States.Woof);

            Assert.AreEqual(stateTwo, _stateManager.GetCurrentState());
        }

    }
}
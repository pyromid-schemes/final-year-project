using AI.States;
using UnityEngine;

namespace AI.MobControllers
{
    abstract class SimpleMobController : MonoBehaviour
    {
        protected StateManager _stateManager;

        // Update is called once per frame
        void Update()
        {
            _stateManager.GetCurrentState().OnUpdate();
        }

        void FixedUpdate()
        {
            _stateManager.GetCurrentState().OnFixedUpdate();
        }
    }
}
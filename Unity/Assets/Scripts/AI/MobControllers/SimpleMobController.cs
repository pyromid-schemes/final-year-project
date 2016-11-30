using AI.States;
using UnityEngine;

namespace AI.MobControllers
{
    abstract class SimpleMobController : MonoBehaviour
    {
        protected StateManager StateManager;
        protected Animator Anim;

        // Update is called once per frame
        void Update()
        {
            StateManager.GetCurrentState().OnUpdate();
        }

        void FixedUpdate()
        {
            StateManager.GetCurrentState().OnFixedUpdate();
        }
    }
}
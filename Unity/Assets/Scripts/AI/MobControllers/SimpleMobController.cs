using AI.States;
using UnityEngine;

namespace AI.MobControllers
{
    abstract class SimpleMobController : MonoBehaviour
    {
        protected IState CurrentState;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CurrentState.OnUpdate();
        }

        void FixedUpdate()
        {
            CurrentState.OnFixedUpdate();
        }
    }
}
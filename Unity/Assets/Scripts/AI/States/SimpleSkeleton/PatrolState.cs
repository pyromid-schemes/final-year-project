//using AI.MobControllers;
//using UnityEngine;

//namespace AI.States.SimpleSkeleton
//{
//    class PatrolState : IState
//    {
//        private readonly SimpleSkeletonController _mob;

//        public PatrolState(SimpleSkeletonController mob)
//        {
//            _mob = mob;
//        }

//        public void OnEnter()
//        {
//        }

//        public void OnUpdate()
//        {
//            RaycastHit hit;
//            if (Physics.Raycast(_mob.transform.position, _mob.transform.forward, out hit) &&
//                hit.collider.CompareTag("Player"))
//            {

//            }
//        }

//        public void OnFixedUpdate()
//        {
//            _mob.transform.Rotate(Vector3.up, Time.deltaTime*_mob.RotateSpeed);
//        }
//    }
//}
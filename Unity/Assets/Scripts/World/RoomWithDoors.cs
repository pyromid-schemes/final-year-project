using System;
using UnityEngine;

namespace World
{
    public class RoomWithDoors : MonoBehaviour
    {
        [System.Serializable]
        public enum DoorPosition { Up, Down, Left, Right}

        public DoorPosition[] doorPositions;
        public GameObject doorObject;

        void Start()
        {
            foreach (DoorPosition d in doorPositions)
            {
                var potentialDoorPosition = ResolveDoorPosition(d);
                RaycastHit raycastHit;
                if (RaycastDoor(potentialDoorPosition, out raycastHit)) {
                    Destroy(raycastHit.transform.parent.gameObject);
                    Debug.Log("shouldn't spawn door and should kill old door");
                }
                else {
                    Instantiate(doorObject, potentialDoorPosition.Position, potentialDoorPosition.Rotation, transform);
                    Debug.Log("should spawn new door");
                }
            }
        }

        private PotentialPosition ResolveDoorPosition(DoorPosition doorPosition)
        {
            Vector3 position = transform.position;
            Vector3 rotation = transform.eulerAngles;
            Vector3 direction = Vector3.zero;

            switch (doorPosition)
            {
                case DoorPosition.Up:
                    position += new Vector3(0, 0, 4.5f);
                    direction = Vector3.forward;
                    break;

                case DoorPosition.Down:
                    position += new Vector3(0, 0, -4.5f);
                    rotation += new Vector3(0, 180, 0);
                    direction = Vector3.back;
                    break;

                case DoorPosition.Left:
                    position += new Vector3(-4.5f, 0, 0);
                    rotation += new Vector3(0, 270, 0);
                    direction = Vector3.left;
                    break;

                case DoorPosition.Right:
                    position += new Vector3(4.5f, 0, 0);
                    rotation += new Vector3(0, 90, 0);
                    direction = Vector3.right;
                    break;
            }
            return new PotentialPosition(position, Quaternion.Euler(rotation), direction);
        }

        private bool RaycastDoor(PotentialPosition potentialPosition, out RaycastHit hit)
        {
            var raycastOrigin = potentialPosition.Position + new Vector3(0, 1, 0);
            Debug.DrawRay(raycastOrigin, potentialPosition.Direction, Color.green, Single.MaxValue);
            bool raycast = Physics.Raycast(raycastOrigin, potentialPosition.Direction, out hit, 1f);
            Debug.Log("raycast hit something: " + raycast);

            return raycast && hit.transform.gameObject.CompareTag("Door");
        }

        struct PotentialPosition
        {
            private Vector3 position;
            private Quaternion rotation;
            private Vector3 direction;

            public PotentialPosition(Vector3 position, Quaternion rotation, Vector3 direction)
            {
                this.position = position;
                this.rotation = rotation;
                this.direction = direction;
            }

            public Vector3 Position
            {
                get { return position; }
                set { position = value; }
            }

            public Quaternion Rotation
            {
                get { return rotation; }
                set { rotation = value; }
            }

            public Vector3 Direction
            {
                get { return direction; }
                set { direction = value; }
            }
        }

    }
}
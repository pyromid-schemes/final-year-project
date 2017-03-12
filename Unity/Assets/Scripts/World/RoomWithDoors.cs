using System;
using System.Collections;
using UnityEngine;

namespace World
{
    public class RoomWithDoors : MonoBehaviour
    {
        [Serializable]
        public enum DoorPosition { Up, Down, Left, Right}

        public DoorPosition[] doorPositions;
        public GameObject doorObject;

        void Start()
        {
            foreach (DoorPosition d in doorPositions)
            {
                var potentialDoorPosition = ResolveDoorPosition(ApplyRoomRotation(d));
                RaycastHit raycastHit;
                if (RaycastDoor(potentialDoorPosition, out raycastHit)) {
                    StartCoroutine(AnimateThenKillDoors(raycastHit.transform.parent.gameObject));
                }
                else {
                    Instantiate(doorObject, potentialDoorPosition.Position, potentialDoorPosition.Rotation, transform);
                }
            }
        }

        private IEnumerator AnimateThenKillDoors(GameObject doors)
        {
            doors.GetComponent<Animator>().Play("TwoDoors");
            yield return new WaitForSeconds(doors.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length + 1);
            Destroy(doors.gameObject);
        }

        private PotentialPosition ResolveDoorPosition(DoorPosition doorPosition)
        {
            Vector3 position = transform.position;
            Vector3 rotation = Vector3.zero;
            Vector3 direction = Vector3.zero;

            switch (doorPosition)
            {
                case DoorPosition.Up:
                    position += new Vector3(0, 0, 4.5f);
                    direction = Vector3.forward;
                    break;

                case DoorPosition.Down:
                    position += new Vector3(0, 0, -4.5f);
                    rotation = new Vector3(0, 180, 0);
                    direction = Vector3.back;
                    break;

                case DoorPosition.Left:
                    position += new Vector3(-4.5f, 0, 0);
                    rotation = new Vector3(0, 270, 0);
                    direction = Vector3.left;
                    break;

                case DoorPosition.Right:
                    position += new Vector3(4.5f, 0, 0);
                    rotation = new Vector3(0, 90, 0);
                    direction = Vector3.right;
                    break;
            }

            return new PotentialPosition(position, Quaternion.Euler(rotation), direction);
        }

        private bool RaycastDoor(PotentialPosition potentialPosition, out RaycastHit hit)
        {
            var raycastOrigin = potentialPosition.Position + new Vector3(0, 1, 0);

            return Physics.Raycast(raycastOrigin, potentialPosition.Direction, out hit, 1f) &&
                   hit.transform.gameObject.CompareTag("Door");
        }

        private DoorPosition ApplyRoomRotation(DoorPosition d)
        {
            var roomRotation = transform.eulerAngles.y;
            switch (d) {
                case DoorPosition.Up:
                    if (Mathf.Approximately(roomRotation, 90f)) {
                        return DoorPosition.Right;
                    }
                    if (Mathf.Approximately(roomRotation, 180f)) {
                        return DoorPosition.Down;
                    }
                    if (Mathf.Approximately(roomRotation, 270f)) {
                        return DoorPosition.Left;
                    }
                    break;

                case DoorPosition.Down:
                    if (Mathf.Approximately(roomRotation, 90f)) {
                        return DoorPosition.Left;
                    }
                    if (Mathf.Approximately(roomRotation, 180f)) {
                        return DoorPosition.Up;
                    }
                    if (Mathf.Approximately(roomRotation, 270f)) {
                        return DoorPosition.Right;
                    }
                    break;

                case DoorPosition.Left:
                    if (Mathf.Approximately(roomRotation, 90f)) {
                        return DoorPosition.Up;
                    }
                    if (Mathf.Approximately(roomRotation, 180f)) {
                        return DoorPosition.Right;
                    }
                    if (Mathf.Approximately(roomRotation, 270f)) {
                        return DoorPosition.Down;
                    }
                    break;

                case DoorPosition.Right:
                    if (Mathf.Approximately(roomRotation, 90f)) {
                        return DoorPosition.Down;
                    }
                    if (Mathf.Approximately(roomRotation, 180f)) {
                        return DoorPosition.Left;
                    }
                    if (Mathf.Approximately(roomRotation, 270f)) {
                        return DoorPosition.Up;
                    }
                    break;
            }
            return d;
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
            }

            public Quaternion Rotation
            {
                get { return rotation; }
            }

            public Vector3 Direction
            {
                get { return direction; }
            }
        }

    }
}
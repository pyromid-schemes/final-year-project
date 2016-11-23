using UnityEngine;
using System.Collections;

namespace Dev
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _body;

        public float Speed = 10f;

        // Use this for initialization
        void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void FixedUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            transform.Translate(movement*Speed);
        }
    }
}
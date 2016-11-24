using UnityEngine;
using System.Collections;

public class MockController : MonoBehaviour {
   
    private CharacterController controller;
    private float speed = 3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;
       
        UpdatePositionOfCharacterIgnoringCollisions(horizontal,vertical);
        UpdateCharactersFowardPositionForCollisions(vertical);
    }

    private void UpdatePositionOfCharacterIgnoringCollisions(float horizontal, float vertical)
    {
        Vector3 newPosition = new Vector3(horizontal *= Time.deltaTime, 0, vertical *= Time.deltaTime);
        transform.Translate(newPosition);
    }

    private void UpdateCharactersFowardPositionForCollisions(float vertical)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * vertical);
    }
}

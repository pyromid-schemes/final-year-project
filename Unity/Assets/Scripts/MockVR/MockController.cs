using UnityEngine;
using System.Collections;

public class MockController : MonoBehaviour
{

    private CharacterController controller;
    private float speed = 3f;
    private const float MeleeMaxRange = 3f;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;

        UpdatePositionOfCharacterIgnoringCollisions(horizontal, vertical);
        UpdateCharactersFowardPositionForCollisions(vertical);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("left mouse down for fire");
            animator.Play("Attack");
        }

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

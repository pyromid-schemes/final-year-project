using UnityEngine;
using System.Collections;

/*
 * @author Daniel Cheng
 * MockController for the Mock VR player which allows the user to use the WASD keys for movement.
 * W for fowards, A for left, S for back, D for right
 * In addition, the user mouse will lock into the screen whilst the mock vr player is in use, 
 * press escape to enable the mouse outside the game
 * This class also handles collision detection thus preventing the Mock VR player from walking through walls 
 */
public class MockController : MonoBehaviour
{
    private CharacterController controller;
    private float speed = 3f;
    private const float MeleeMaxRange = 3f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        DisableAndLockMouseCursorToScreen();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;

        UpdatePositionOfCharacterIgnoringCollisions(horizontal, vertical);
        UpdateCharactersFowardPositionForCollisions(vertical);
        IfEscapeIsPressedEnableMouseCursor();
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

    private void DisableAndLockMouseCursorToScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void IfEscapeIsPressedEnableMouseCursor()
    {
        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


}

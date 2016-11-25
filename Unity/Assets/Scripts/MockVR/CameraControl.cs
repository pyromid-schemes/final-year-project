using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    private Vector2 mouseLook;
    private float sensitivity = 5f;
    private float smoothing = 2f;

    private GameObject character;

	void Start ()
    {
        character = this.transform.parent.gameObject;    
	}
	
	void Update ()
    {

    }

    void FixedUpdate()
    {
        Vector2 currentMouseMovement = GetCurrentMouseMovement();
   
        UpdateCameraPosition(currentMouseMovement.x,currentMouseMovement.y);
        RotateCameraYAxis();
        RotateParentObjectXAxis();
    }

    private Vector2 GetCurrentMouseMovement()
    {
        Vector2 currentMouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        float smoothingRate = sensitivity * smoothing;
        currentMouseMovement = Vector2.Scale(currentMouseMovement, new Vector2(smoothingRate, smoothingRate));

        return currentMouseMovement;
    }

    private void UpdateCameraPosition(float xAxis, float yAxis)
    {
        Vector2 smoothCameraMovement = new Vector2();
        smoothCameraMovement.x = Mathf.Lerp(smoothCameraMovement.x, xAxis, 1f / smoothing);
        smoothCameraMovement.y = Mathf.Lerp(smoothCameraMovement.y, yAxis, 1f / smoothing);
        mouseLook += smoothCameraMovement;
    }

    private void RotateCameraYAxis()
    {
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
    }

    private void RotateParentObjectXAxis()
    {
        Rigidbody rigibody = character.GetComponentInParent<Rigidbody>();
        rigibody.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}

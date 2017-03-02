using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class CollisionTester : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("On Trigger Enter: " + other);
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("On Trigger Stay: " + other);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("On Trigger Exit: " + other);
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("On Collision Enter: " + other);
    }

    void OnCollisionStay(Collision other)
    {
        Debug.Log("On Collision Stay: " + other);
    }

    void OnCollisionExit(Collision other)
    {
        Debug.Log("On Collision Enter: " + other);
    }
}

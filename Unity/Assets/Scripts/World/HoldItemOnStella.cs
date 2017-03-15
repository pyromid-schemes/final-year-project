using UnityEngine;
using System.Collections;

public class HoldItemOnStella : MonoBehaviour {
	
    void Awake()
    {
        FreeChildren();
    }

    void FixedUpdate()
    {
        FloatItemAbove();
    }

    void FloatItemAbove()
    {
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, Vector3.up, out hit, maxDistance: 1.2f))
        {
            if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
            {
                hit.rigidbody.velocity = Vector3.zero;
                hit.rigidbody.angularVelocity = Vector3.zero;
                hit.rigidbody.Sleep();
            }
        }
    }

    void FreeChildren()
    {
        foreach(Transform child in gameObject.transform)
        {
            child.parent = null;
        }
    }
}

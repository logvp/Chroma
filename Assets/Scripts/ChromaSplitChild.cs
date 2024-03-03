using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaSplitChild : MonoBehaviour
{
    private bool wasKinematic;

    void OnEnable()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            wasKinematic = rb.isKinematic;
            rb.detectCollisions = false;
        }
    }

    public void MakeReal()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = wasKinematic;
            rb.detectCollisions = true;
        }
    }
}

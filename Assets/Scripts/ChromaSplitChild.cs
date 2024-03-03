using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaSplitChild : MonoBehaviour
{
    private bool wasKinematic;

    void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            wasKinematic = rb.isKinematic;
            rb.detectCollisions = false;
        }
    }

    public void MakeReal()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = wasKinematic;
            rb.detectCollisions = true;
        }
    }
}

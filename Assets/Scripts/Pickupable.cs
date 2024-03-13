using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public Rigidbody rb;
    private float angularDrag;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        angularDrag = rb.angularDrag;
    }

    public void OnPickUp()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.angularDrag = angularDrag * 2 + 1;
    }

    public void OnPutDown()
    {
        rb.useGravity = true;
        rb.angularDrag = angularDrag;
    }
}

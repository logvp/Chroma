using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    public Transform head;
    public Transform target;
    public float interactDistance;
    public float forceMultiplier;

    public Pickupable thingWePickUp = null;

    private Rigidbody rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (thingWePickUp != null)
            {
                thingWePickUp.OnPutDown();
                thingWePickUp = null;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(head.position, head.forward, out hit, interactDistance, ~((1<<2)|(1<<8))))
                {
                    GameObject obj = hit.transform.gameObject;
                    Debug.Log("Clicked on " + obj.name);
                    Pickupable item = obj.GetComponent<Pickupable>();
                    if (item != null)
                    {
                        item.OnPickUp();
                        thingWePickUp = item;
                        thingWePickUp.transform.position = target.position;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (thingWePickUp != null)
        {
            Vector3 dir = target.position - thingWePickUp.transform.position;
            if (dir.magnitude > 2)
            {
                dir = dir.normalized * 2;
            }
            thingWePickUp.rb.velocity = dir * forceMultiplier + rb.velocity * 0.5f;
        }
    }
}

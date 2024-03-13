using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjects : MonoBehaviour
{
    public Transform head;
    public Transform target;
    public float interactDistance;
    public float dropDistance;
    public float forceMultiplier;
    public float rightingStrength;
    public float maxSpeed;

    public Pickupable thingWePickUp = null;

    private Rigidbody rb;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (GameState.isPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (thingWePickUp != null)
            {
                DropItem();
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(head.position, head.forward, out hit, interactDistance, ~((1<<2)|(1<<8))))
                {
                    GameObject obj = hit.transform.gameObject;
                    Pickupable item = obj.GetComponent<Pickupable>();
                    if (item != null)
                    {
                        item.OnPickUp();
                        thingWePickUp = item;
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
            float distance = dir.magnitude;
            if (distance > dropDistance || thingWePickUp.rb.isKinematic)
            {
                DropItem();
                return;
            }
            if (distance > maxSpeed)
            {
                dir = dir.normalized * maxSpeed;
            }
            thingWePickUp.rb.velocity = dir * forceMultiplier + rb.velocity * 0.5f;
            // https://forum.unity.com/threads/how-to-make-a-rigidbody-turn-upright.44841/
            Vector3 tug = Vector3.up * rightingStrength;
            thingWePickUp.rb.AddForceAtPosition(thingWePickUp.transform.TransformPoint(Vector3.up), tug);
            thingWePickUp.rb.AddForceAtPosition(thingWePickUp.transform.TransformPoint(-Vector3.up), -tug);
        }
    }

    private void DropItem()
    {
        if (thingWePickUp != null)
        {
            thingWePickUp.OnPutDown();
            thingWePickUp = null;
        }
    }
}

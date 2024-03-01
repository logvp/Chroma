using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed, jumpForce;
    public float jumpRaycastDistance;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal") * baseSpeed;
        float z = Input.GetAxisRaw("Vertical") * baseSpeed;

        Vector3 dir = transform.right * x + transform.forward * z;
        Vector3 vel = new Vector3(dir.x, rb.velocity.y, dir.z);

        rb.velocity = vel;

        if (Input.GetKey(KeyCode.Space))
        {
            if (Physics.Linecast(transform.position, new Vector3(transform.position.x, transform.position.y - jumpRaycastDistance, transform.position.z)))
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
        }
    }
}

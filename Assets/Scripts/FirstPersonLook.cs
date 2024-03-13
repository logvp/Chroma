using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public float sensitivity;
    public Transform head;
    float x, y;

    void Start()
    {
        Debug.Assert(head != null, "FirstPersonLook does not have a reference to the head/camera");
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (GameState.isPaused) return;

        x -= Input.GetAxis("Mouse Y") * sensitivity;
        y += Input.GetAxis("Mouse X") * sensitivity;
        x = Mathf.Clamp(x, -90, 90);

        head.localRotation = Quaternion.Euler(x, 0, 0);
        transform.localRotation = Quaternion.Euler(0, y, 0);
    }
}

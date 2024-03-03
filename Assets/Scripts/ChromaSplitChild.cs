using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaSplitChild : MonoBehaviour
{
    public ChromaSplitParent parent;

    // private bool wasKinematic;

    void OnEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        MakeNotReal();
    }

    public void MakeNotReal()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // TODO remember if it was kinematic
            rb.isKinematic = true;
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
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
    }

    public void UpdateColor(ChromaColor color)
    {
        Renderer myRenderer = GetComponent<Renderer>();
        switch (color)
        {
            case ChromaColor.Red:
                gameObject.layer = 10;
                myRenderer.material = GameState.RedMat;
                break;
            case ChromaColor.Green:
                gameObject.layer = 11;
                myRenderer.material = GameState.GreenMat;
                break;
            case ChromaColor.Blue:
                gameObject.layer = 12;
                myRenderer.material = GameState.BlueMat;
                break;
            // case ChromaColor.White:
            //     myRenderer.material = parent.baseMaterial;
            //     break;
            default:
                Debug.LogAssertion("Unreachable case");
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLights : MonoBehaviour, ButtonEvent
{
    public Material offMat, onMat;

    private Renderer myRenderer;

    void OnEnable()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.material = offMat;
    }

    public void EndButtonEvent()
    {
        myRenderer.material = offMat;
    }

    public void StartButtonEvent()
    {
        myRenderer.material = onMat;
    }
}

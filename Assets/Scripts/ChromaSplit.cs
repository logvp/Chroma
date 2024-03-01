using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaSplit : MonoBehaviour
{
    public enum ChromaColor
    {
        Red,
        Green,
        Blue,
        White,
    }

    public bool splitable = true;

    public float timeToSplit = 2;

    private bool isSplitting;
    private float splitStart;

    private Renderer myRenderer;
    private Material baseMaterial;
    public ChromaColor color = ChromaColor.White;

    private GameObject leftClone, rightClone;

    void OnEnable()
    {
        myRenderer = GetComponent<Renderer>();
        baseMaterial = myRenderer.material;
        Debug.Assert(myRenderer != null);
        isSplitting = false;
        splitStart = 0;
        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSplitting) return;

        float t = (Time.time - splitStart) / timeToSplit;
        leftClone.transform.position = Vector3.Lerp(transform.position, transform.position - GameState.Player.transform.right, t);
        rightClone.transform.position = Vector3.Lerp(transform.position, transform.position + GameState.Player.transform.right, t);

        if (t > 1)
        {
            FinishSplitting();
        }
    }

    private void StartSplitting()
    {
        if (!splitable) return;

        isSplitting = true;
        splitStart = Time.time;
        
        if (leftClone == null)
        {
            leftClone = Instantiate(gameObject, transform.position, transform.rotation);
            ChromaSplit script = leftClone.GetComponent<ChromaSplit>();
            script.splitable = false;
            script.UpdateColor(ChromaColor.Red);
        }
        if (rightClone == null)
        {
            rightClone = Instantiate(gameObject, transform.position, transform.rotation);
            ChromaSplit script = rightClone.GetComponent<ChromaSplit>();
            script.splitable = false;
            script.UpdateColor(ChromaColor.Blue);
        }
        UpdateColor(ChromaColor.Green);
    }

    private void AbortSplitting()
    {
        if (!isSplitting) return;

        isSplitting = false;
        if (leftClone != null)
        {
            Destroy(leftClone);
            leftClone = null;
        }
        if (rightClone != null)
        {
            Destroy(rightClone);
            rightClone = null;
        }
        UpdateColor(ChromaColor.White);
    }

    private void FinishSplitting()
    {
        isSplitting = false;
        splitable = false;
        leftClone = null;
        rightClone = null;
    }

    private void UpdateColor(ChromaColor color)
    {
        this.color = color;
        UpdateColor();
    }

    private void UpdateColor()
    {
        switch (color)
        {
            case ChromaColor.Red:
                myRenderer.material = GameState.RedMat;
                break;
            case ChromaColor.Green:
                myRenderer.material = GameState.GreenMat;
                break;
            case ChromaColor.Blue:
                myRenderer.material = GameState.BlueMat;
                break;
            case ChromaColor.White:
                myRenderer.material = baseMaterial;
                break;
            default:
                Debug.LogAssertion("Unreachable case");
                break;
        }
    }

    void OnMouseDown()
    {
        StartSplitting();
    }

    void OnMouseUp()
    {
        AbortSplitting();
    }
}

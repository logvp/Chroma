using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaSplit : MonoBehaviour
{
    public bool splitable = true;

    public float timeToSplit = 2;

    private bool isSplitting;
    private float splitStart;

    // public Material rMat, gMat, bMat;

    private GameObject leftClone, rightClone;

    // Start is called before the first frame update
    void Start()
    {
        isSplitting = false;
        splitStart = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSplitting) return;

        float t = (Time.time - splitStart) / timeToSplit;
        Debug.Log(t);
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
        }
        if (rightClone == null)
        {
            rightClone = Instantiate(gameObject, transform.position, transform.rotation);
        }
    }

    private void AbortSplitting()
    {
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
    }

    private void FinishSplitting()
    {
        isSplitting = false;
        leftClone = null;
        rightClone = null;
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

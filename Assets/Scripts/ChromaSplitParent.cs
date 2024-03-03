using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaSplitParent : MonoBehaviour
{
    public enum ChromaColor
    {
        Red,
        Green,
        Blue,
        White,
    }

    public enum SplittingState
    {
        NotSplitting,
        Separating,
        Rejoining,
        DoneSplitting,
    }

    public bool primary = true;
    public float timeToSplit = 2;
    public float rejoinRate = 1;

    private bool hasInput = false;
    public SplittingState state;
    private float timeSplitting;
    private Renderer myRenderer;
    private Material baseMaterial;

    private GameObject[] copies;

    void OnEnable()
    {
        myRenderer = GetComponent<Renderer>();
        baseMaterial = myRenderer.material;
        timeSplitting = 0;
        state = SplittingState.NotSplitting;
    }

    void Start()
    {
        GameObject[] copies = new GameObject[3];
        enabled = false;
        for (int i = 0; i < 3; i++)
        {
            copies[i] = Instantiate(gameObject, transform.position, transform.rotation);
            Debug.Assert(copies[i] != null);
            Renderer renderer = copies[i].GetComponent<Renderer>();
            Debug.Assert(renderer != null);
            renderer.material = i switch
            {
                0 => GameState.RedMat,
                1 => GameState.GreenMat,
                2 => GameState.BlueMat,
                _ => throw new System.ArgumentException(),
            };
            ChromaSplitChild script = copies[i].GetComponent<ChromaSplitChild>();
            Debug.Assert(script != null);
            script.enabled = true;
            copies[i].SetActive(false);
        }
        enabled = true;
        this.copies = copies;
    }

    // Update is called once per frame
    void Update()
    {
        StateTransition();
        switch (state)
        {
            case SplittingState.NotSplitting:
                break;
            case SplittingState.Separating:
                timeSplitting += Time.deltaTime;
                UpdateCopyPositions();
                break;
            case SplittingState.Rejoining:
                timeSplitting -= Time.deltaTime * rejoinRate;
                UpdateCopyPositions();
                break;
            case SplittingState.DoneSplitting:
                break;
        }
    }

    private void StateTransition()
    {
        // Update State
        switch (state)
        {
            case SplittingState.NotSplitting:
                // 0 => NotSplitting
                // 1 => Separating
                if (hasInput)
                {
                    GoToSeparating();
                }
                break;
            case SplittingState.Separating:
                // 0 => Rejoining
                // 1 => DoneSplitting
                if (!hasInput)
                {
                    GoToRejoining();
                }
                else if (hasInput && timeSplitting >= timeToSplit)
                {
                    GoToDoneSplitting();
                }
                break;
            case SplittingState.Rejoining:
                // 0 => NotSplitting
                // 1 => Separating
                if (hasInput)
                {
                    GoToSeparating();
                }
                else if (!hasInput && timeSplitting <= 0)
                {
                    GoToNotSplitting();
                }
                break;
            case SplittingState.DoneSplitting:
                // 0 => DoneSplitting
                // 1 => DoneSplitting
                break;
        }
    }

    private void UpdateCopyPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            copies[i].transform.position = i switch
            {
                0 => -GameState.Player.transform.right,
                2 => GameState.Player.transform.right,
                _ => Vector3.zero,
            } * (timeSplitting / timeToSplit) + transform.position;
            copies[i].transform.rotation = transform.rotation;
        }
    }

    private void GoToSeparating()
    {
        state = SplittingState.Separating;
        foreach (GameObject copy in copies)
        {
            copy.SetActive(true);
        }
        myRenderer.enabled = false;
    }

    private void GoToNotSplitting()
    {
        timeSplitting = 0;
        state = SplittingState.NotSplitting;
        foreach (GameObject copy in copies)
        {
            copy.SetActive(false);
        }
        myRenderer.enabled = true;
    }

    private void GoToDoneSplitting()
    {
        state = SplittingState.DoneSplitting;
        gameObject.SetActive(false);
        foreach (GameObject copy in copies) {
            copy.GetComponent<ChromaSplitChild>().MakeReal();
        }
    }

    private void GoToRejoining()
    {
        state = SplittingState.Rejoining;
    }

    // private void UpdateColor()
    // {
    //     switch (color)
    //     {
    //         case ChromaColor.Red:
    //             myRenderer.material = GameState.RedMat;
    //             break;
    //         case ChromaColor.Green:
    //             myRenderer.material = GameState.GreenMat;
    //             break;
    //         case ChromaColor.Blue:
    //             myRenderer.material = GameState.BlueMat;
    //             break;
    //         case ChromaColor.White:
    //             myRenderer.material = baseMaterial;
    //             break;
    //         default:
    //             Debug.LogAssertion("Unreachable case");
    //             break;
    //     }
    // }

    void OnMouseDown()
    {
        // StartSplitting();
        hasInput = true;
    }

    void OnMouseUp()
    {
        // AbortSplitting();
        hasInput = false;
    }
}

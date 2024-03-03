using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ChromaColor
{
    Red,
    Green,
    Blue,
    White,
}

public class ChromaSplitParent : MonoBehaviour
{
    private enum SplittingState
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
    private SplittingState state;
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
        gameObject.layer = 9;
        GameObject[] copies = new GameObject[3];
        enabled = false;
        for (int i = 0; i < 3; i++)
        {
            copies[i] = Instantiate(gameObject, transform.position, transform.rotation);
            ChromaSplitChild script = copies[i].GetComponent<ChromaSplitChild>();
            script.enabled = true;
            script.parent = this;
            script.UpdateColor(i switch
            {
                0 => ChromaColor.Red,
                1 => ChromaColor.Green,
                2 => ChromaColor.Blue,
                _ => throw new System.ArgumentException(),
            });
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

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
    public enum SplittingState
    {
        NotSplitting,
        Separating,
        Rejoining,
        DoneSplitting,
        Recombining,
    }

    public bool primary = true;
    public float timeToSplit = 2;
    public float rejoinRate = 2;
    public float recombineSpeed = 2;
    public float beginRecombineThreshold = 1;
    public float finishRecombineThreshold = 0.1f;

    public float inputDistanceCutoff = 6;
    public float inputAngleCutoff = 0.95f;

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
        GetInput();
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
            case SplittingState.Recombining:
                // Handled in FixedUpdate
                break;
        }
    }

    void FixedUpdate()
    {
        if (state == SplittingState.Recombining)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 dir = (transform.position - copies[i].transform.position).normalized;
                copies[i].transform.position += dir * recombineSpeed * Time.fixedDeltaTime;
                copies[i].transform.rotation = transform.rotation;
            }
        }
    }

    private void GetInput()
    {
        hasInput = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 dir = transform.position - GameState.Player.transform.position;
            if (dir.magnitude < inputDistanceCutoff)
            {
                if (Vector3.Dot(GameState.PlayerHead.transform.forward, dir.normalized) > inputAngleCutoff)
                {
                    Debug.Log("angle check passes");
                    hasInput = true;
                }
            }
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
                {
                    Vector3 a = copies[0].transform.position - copies[1].transform.position;
                    Vector3 b = copies[0].transform.position - copies[2].transform.position;
                    Vector3 c = copies[1].transform.position - copies[2].transform.position;
                    if (a.magnitude < beginRecombineThreshold && b.magnitude < beginRecombineThreshold && c.magnitude < beginRecombineThreshold)
                    {
                        GoToRecombining();
                    }
                } break;
            case SplittingState.Recombining:
                {
                    Vector3 a = copies[0].transform.position - copies[1].transform.position;
                    Vector3 b = copies[0].transform.position - copies[2].transform.position;
                    Vector3 c = copies[1].transform.position - copies[2].transform.position;
                    if (a.magnitude < finishRecombineThreshold && b.magnitude < finishRecombineThreshold && c.magnitude < finishRecombineThreshold)
                    {
                        GoToNotSplitting();
                    }
                } break;
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
        Hide();
    }

    private void GoToNotSplitting()
    {
        state = SplittingState.NotSplitting;
        timeSplitting = 0;
        foreach (GameObject copy in copies)
        {
            copy.SetActive(false);
        }
        Enable();
    }

    private void GoToDoneSplitting()
    {
        state = SplittingState.DoneSplitting;
        foreach (GameObject copy in copies) {
            copy.GetComponent<ChromaSplitChild>().MakeReal();
        }
        Disable();
    }

    private void GoToRejoining()
    {
        state = SplittingState.Rejoining;
        foreach (GameObject copy in copies)
        {
            copy.GetComponent<ChromaSplitChild>().MakeNotReal();
        }
    }

    private void GoToRecombining()
    {
        state = SplittingState.Recombining;
        transform.position = (copies[0].transform.position + copies[1].transform.position + copies[2].transform.position) / 3.0f;
        Disable();
    }

    private void Disable()
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
        Hide();
    }

    private void Enable()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
        Show();
    }

    private void Hide()
    {
        myRenderer.enabled = false;
    }

    private void Show()
    {
        myRenderer.enabled = true;
    }
}

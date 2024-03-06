using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour, ButtonEvent
{
    private enum State
    {
        Closed,
        Closing,
        Open,
        Opening,
    }

    public Transform close, open;
    public float timeToOpen = 1;
    public float timeToClose = 0.5f;

    // [0, 1]
    private float stateTime;
    private bool isPowered;
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        stateTime = 0;
        isPowered = false;
        state = State.Closed;
    }

    // Update is called once per frame
    void Update()
    {
        // Update state
        switch (state)
        {
            case State.Closed:
                if (isPowered)
                {
                    state = State.Opening;
                }
                break;
            case State.Closing:
                if (isPowered)
                {
                    state = State.Opening;
                }
                else if (stateTime <= 0)
                {
                    stateTime = 0;
                    state = State.Closed;
                }
                break;
            case State.Open:
                if (!isPowered)
                {
                    state = State.Closing;
                }
                break;
            case State.Opening:
                if (!isPowered)
                {
                    state = State.Closing;
                }
                else if (stateTime >= 1)
                {
                    stateTime = 1;
                    state = State.Open;
                } 
                break;
        }
        // Apply state
        switch (state)
        {
            case State.Closed:
                transform.position = close.position;
                break;
            case State.Closing:
                stateTime -= Time.deltaTime / timeToClose;
                transform.position = Vector3.Lerp(close.position, open.position, stateTime);
                break;
            case State.Open:
                transform.position = open.position;
                break;
            case State.Opening:
                stateTime += Time.deltaTime / timeToOpen;
                transform.position = Vector3.Lerp(close.position, open.position, stateTime);
                break;
        }
    }

    public void EndButtonEvent()
    {
        isPowered = false;
    }

    public void StartButtonEvent()
    {
        isPowered = true;
    }
}

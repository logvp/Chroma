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
    public int powerRequired = 1;
    public float timeToOpen = 1;
    public float timeToClose = 0.5f;

    // [0, 1]
    private float stateTime;
    private int powerSources;
    private State state;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        stateTime = 0;
        powerSources = 0;
        state = State.Closed;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update state
        switch (state)
        {
            case State.Closed:
                if (powerSources >= powerRequired)
                {
                    Transition(State.Opening);
                }
                break;
            case State.Closing:
                if (powerSources >= powerRequired)
                {
                    Transition(State.Opening);
                }
                else if (stateTime <= 0)
                {
                    stateTime = 0;
                    Transition(State.Closed);
                }
                break;
            case State.Open:
                if (powerSources < powerRequired)
                {
                    Transition(State.Closing);
                }
                break;
            case State.Opening:
                if (powerSources < powerRequired)
                {
                    Transition(State.Closing);
                }
                else if (stateTime >= 1)
                {
                    stateTime = 1;
                    Transition(State.Open);
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

    private void Transition(State state)
    {
        if (state == State.Closing || state == State.Opening)
        {
            source.Play();
        }
        this.state = state;
    }

    public void EndButtonEvent()
    {
        powerSources -= 1;
    }

    public void StartButtonEvent()
    {
        powerSources += 1;
    }
}

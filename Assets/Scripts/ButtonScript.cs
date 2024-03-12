using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject[] events;

    private AudioSource source;
    private ButtonEvent[] buttonEvents;
    private int numInteractions;

    void OnEnable()
    {
        source = GetComponent<AudioSource>();
        buttonEvents = new ButtonEvent[events.Length];
        for (int i = 0; i < events.Length; i++)
        {
            buttonEvents[i] = events[i].GetComponent<ButtonEvent>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if ((layerMask.value & (1 << obj.layer)) != 0)
        {
            if (numInteractions == 0)
            {
                source.Play();
                foreach (ButtonEvent e in buttonEvents)
                {
                    e.StartButtonEvent();
                }
            }
            numInteractions++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if ((layerMask.value & (1 << obj.layer)) != 0)
        {
            // TODO this is not consistent
            numInteractions--;
            if (numInteractions == 0)
            {
                foreach (ButtonEvent e in buttonEvents)
                {
                    e.EndButtonEvent();
                }
            }
        }
    }
}

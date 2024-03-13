using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassesScript : MonoBehaviour
{
    public float rotationSpeed;
    public GameObject[] buttonEvents;
    private ButtonEvent[] events;

    // Start is called before the first frame update
    void Start()
    {
        events = new ButtonEvent[buttonEvents.Length];
        for (int i = 0; i < buttonEvents.Length; i++) 
        {
            events[i] = buttonEvents[i].GetComponent<ButtonEvent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Start the game
            foreach (ButtonEvent e in events)
            {
                e.StartButtonEvent();
            }

            gameObject.SetActive(false);
        }
    }
}

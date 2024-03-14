using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    public VoiceLines.Lines whatAmI;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (GameState.VoiceLines.GetTriggerInput(whatAmI))
            {
                gameObject.SetActive(false);
            }
        }
    }
}

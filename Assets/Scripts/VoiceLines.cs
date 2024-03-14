using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{
    public enum Lines {
        NotDoingStory,
        Chamber0,
        Chamber1,
        Chamber2,
        Chamber3,
        Chamber5,
        Cake,
    }
    private Lines whatComesNext = Lines.NotDoingStory;

    public AudioClip chamber0;
    public AudioClip chamber1;
    public AudioClip chamber2;
    public AudioClip chamber3;
    public AudioClip chamber5;
    public AudioClip cake;

    private AudioSource source;

    public void WereDoingStory()
    {
        source = GameState.Player.GetComponent<AudioSource>();
        source.PlayOneShot(chamber0, 1);
        whatComesNext = Lines.Chamber1;
    }

    public bool GetTriggerInput(Lines line)
    {
        if (line == whatComesNext)
        {
            source.PlayOneShot(line switch
            {
                Lines.Chamber0 => chamber0,
                Lines.Chamber1 => chamber1,
                Lines.Chamber2 => chamber2,
                Lines.Chamber3 => chamber3,
                Lines.Chamber5 => chamber5,
                Lines.Cake => cake,
                _ => throw new System.ArgumentException(),
            }, 1);
            whatComesNext = line switch
            {
                Lines.Chamber0 => Lines.Chamber1,
                Lines.Chamber1 => Lines.Chamber2,
                Lines.Chamber2 => Lines.Chamber3,
                Lines.Chamber3 => Lines.Chamber5,
                Lines.Chamber5 => Lines.Cake,
                Lines.Cake => Lines.NotDoingStory,
                _ => throw new System.ArgumentException(),
            };
            return true;
        }
        return false;
    }
}

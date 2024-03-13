using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioClip clip;
    public AudioSource source;

    public void OnPointerClick(PointerEventData eventData)
    {
        source.PlayOneShot(clip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        source.PlayOneShot(clip);
    }
}

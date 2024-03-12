using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticBend : MonoBehaviour
{
    public Volume volume;
    public float inTime, outTime;

    private ChromaticAberration effect;
    private float effectStrength;

    // Start is called before the first frame update
    void Start()
    {
        effectStrength = 0;
        volume.profile.TryGet(out effect);
        Debug.Assert(effect != null);
    }

    // Update is called once per frame
    void Update()
    {
        if (ChromaSplitParent.GetChromaKeyInput())
        {
            effectStrength += Time.deltaTime / inTime;
        }
        else
        {
            effectStrength -= (effectStrength / 2) * Time.deltaTime;
            effectStrength -= Time.deltaTime / outTime;
            effectStrength = Mathf.Max(effectStrength, 0);
        }

        float appliedStrength = 1f - (1f / (effectStrength + 1f));
        appliedStrength = Mathf.Clamp01(appliedStrength);
        effect.intensity.value = appliedStrength;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticBend : MonoBehaviour
{
    public Volume volume;
    public float inTime, outTime;
    public float minSaturation, maxSaturation;

    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;
    private float effectStrength;

    // Start is called before the first frame update
    void Start()
    {
        effectStrength = 0;
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out colorAdjustments);
        Debug.Assert(chromaticAberration != null);
        Debug.Assert(colorAdjustments != null);
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
        chromaticAberration.intensity.value = appliedStrength;
        colorAdjustments.saturation.value = Mathf.Lerp(minSaturation, maxSaturation, appliedStrength);
    }
}

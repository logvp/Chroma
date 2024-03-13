using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticBend : MonoBehaviour
{
    public Volume volume;
    public float inTime, outTime;
    public float minAberration, maxAberration;
    public float minSaturation, maxSaturation;
    public float minBloom, maxBloom;

    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;
    private Bloom bloom;
    private float effectStrength;

    // Start is called before the first frame update
    void Start()
    {
        effectStrength = 0;
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out bloom);
        Debug.Assert(chromaticAberration != null);
        Debug.Assert(colorAdjustments != null);
        Debug.Assert(bloom != null);
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

        float normalizedStrength = 1f - (1f / (effectStrength + 1f));
        normalizedStrength = Mathf.Clamp01(normalizedStrength);

        chromaticAberration.intensity.value = Mathf.Lerp(minAberration, maxAberration, normalizedStrength);
        colorAdjustments.saturation.value = Mathf.Lerp(minSaturation, maxSaturation, normalizedStrength);
        bloom.intensity.value = Mathf.Lerp(minBloom, maxBloom, normalizedStrength);
    }
}

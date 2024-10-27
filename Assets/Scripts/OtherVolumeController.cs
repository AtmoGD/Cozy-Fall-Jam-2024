using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OtherVolumeController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [SerializeField] private float chromaticSpeed = 1f;
    [SerializeField] private float chromaticMin = 0.5f;
    [SerializeField] private float chromaticMax = 1f;
    [SerializeField] private float chromaticRandomnessMin = 0.5f;
    [SerializeField] private float chromaticRandomnessMax = 1f;
    [SerializeField] private float chromaticRandomness => UnityEngine.Random.Range(chromaticRandomnessMin, chromaticRandomnessMax);
    [SerializeField] private float lensDistortionSpeed = 1f;
    [SerializeField] private float lensDistortionMin = -0.5f;
    [SerializeField] private float lensDistortionMax = 1f;
    [SerializeField] private float lensDistortionRandomnessMin = 0.5f;
    [SerializeField] private float lensDistortionRandomnessMax = 1f;
    [SerializeField] private float lensDistortionRandomness => UnityEngine.Random.Range(lensDistortionRandomnessMin, lensDistortionRandomnessMax);
    [SerializeField] private bool active = false;

    private void Update()
    {
        volume.enabled = active;

        if (!active) return;


        if (volume.profile.TryGet(out ChromaticAberration chromaticAberration))
        {
            chromaticAberration.intensity.value = Remap01(Mathf.PingPong((Time.time + chromaticRandomness) * chromaticSpeed, 1), chromaticMin, chromaticMax);
        }

        if (volume.profile.TryGet(out LensDistortion lensDistortion))
        {
            lensDistortion.intensity.value = Remap01(Mathf.PingPong((Time.time + lensDistortionRandomness) * lensDistortionSpeed, 1), lensDistortionMin, lensDistortionMax);
        }
    }

    public void SetEffectActive(bool active)
    {
        this.active = active;
    }

    private float Remap01(float value, float from1, float to1)
    {
        return Remap(value, 0, 1, from1, to1);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

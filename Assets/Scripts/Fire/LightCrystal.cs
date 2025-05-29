using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CrystalLightPulse : MonoBehaviour
{
    public Light2D light2D;
    public float baseIntensity = 1f;
    public float pulseSpeed = 2f;
    public float pulseRange = 0.4f;

    void Update()
    {
        light2D.intensity = baseIntensity + Mathf.Sin(Time.time * pulseSpeed) * pulseRange;
    }
}

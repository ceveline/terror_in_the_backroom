using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightFlicker : MonoBehaviour
{
    private Light lightSource;
    public float minIntensity = 0.5f;  // Minimum light intensity
    public float maxIntensity = 2.0f; // Maximum light intensity
    public float flickerSpeed = 0.1f; // Speed of flickering

    private float targetIntensity;
    private float smoothTime = 0.1f; // Smoothing time for transitions
    private float intensityVelocity; // Used for smooth damping

    void Start()
    {
        // Get the Light component attached to this GameObject
        lightSource = GetComponent<Light>();
        targetIntensity = lightSource.intensity;
    }

    void Update()
    {
        // Randomly change the target intensity
        if (Random.value > 0.9f) // Adjust probability for flicker frequency
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        // Smoothly transition to the target intensity
        lightSource.intensity = Mathf.SmoothDamp(
            lightSource.intensity,
            targetIntensity,
            ref intensityVelocity,
            smoothTime
        );
    }
}

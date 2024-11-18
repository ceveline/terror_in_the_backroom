using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlickeringLight : MonoBehaviour
{
    private Image overlayImage;
    public float minAlpha = 0.1f;        // Minimum opacity
    public float maxAlpha = 0.6f;        // Maximum opacity
    public float flickerSpeed = 3f;    // Speed of flickering

    private void Start()
    {
        overlayImage = GetComponent<Image>();
    }

    private void Update()
    {
        // Smoothly transition the alpha using Perlin noise for a random flickering effect
        Color color = overlayImage.color;
        color.a = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
        overlayImage.color = color;
    }
}

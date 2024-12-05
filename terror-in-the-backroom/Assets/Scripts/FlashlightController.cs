using System.Collections;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public GameObject flashlight;
    public AudioSource turnOn;
    public AudioSource turnOff;

    private bool isOn;
    private bool canToggle = true;
    private float flashlightDuration = 15f;  // Time the flashlight stays on
    private float cooldownDuration = 15f;    // Cooldown time before it can be toggled again

    void Start()
    {
        flashlight.SetActive(false);
        isOn = false;
    }

    void Update()
    {
        if (canToggle && Input.GetButtonDown("F"))
        {
            if (isOn)
            {
                TurnOffFlashlight();
            }
            else
            {
                StartCoroutine(FlashlightTimer());
            }
        }
    }

    private void TurnOffFlashlight()
    {
        flashlight.SetActive(false);
        turnOff.Play();
        isOn = false;
        StartCoroutine(CooldownTimer());
    }

    private IEnumerator FlashlightTimer()
    {
        flashlight.SetActive(true);
        turnOn.Play();
        isOn = true;

        yield return new WaitForSeconds(flashlightDuration);

        TurnOffFlashlight();  // Automatically turn off after 10 seconds
    }

    private IEnumerator CooldownTimer()
    {
        canToggle = false;  // Disable toggling during cooldown
        yield return new WaitForSeconds(cooldownDuration);
        canToggle = true;   // Re-enable toggling after cooldown
    }
}

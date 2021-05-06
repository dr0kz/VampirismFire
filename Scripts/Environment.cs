using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    #region Singleton
    public static Environment ENVIRONMENT;
    #endregion
    [SerializeField] private GameObject day; // Day sky
    [SerializeField] private GameObject night; // Night sky
    [SerializeField] private AudioClip torchSoundEffect; //Torch fire trigger sound effect
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<GameObject> torches; // List of torches in the scene
    private const float maxFogDensity = 0.06f; // Max fog density
    private const float dayLightMaxIntensity = 4.4f;// Light intensity of day sky
    private const float nightLightMaxIntensity = 250.0f; // Light intensity of night sky
    private const float dayDuration = 20f; //Duration of day in seconds
    private const float nightDuration = 8f; //Duration of night in seconds
    /// <summary>
    /// Called when the script is loaded
    /// </summary>
    private void Awake()
    {
        ENVIRONMENT = this;
    }
    /// <summary>
    /// Called on the first frame
    /// </summary>
    private void Start()
    {
        StartCoroutine("DecreaseDayIncreaseNight");
    }
    /// <summary>
    /// Hides the fire from the torches and plays the fire sound effect
    /// </summary>
    private void DisableFireTorches()
    {
        torches.ForEach(torch => {
            torch.SetActive(false);
            audioSource.PlayOneShot(torchSoundEffect);
        });
    }
    /// <summary>
    /// Shows the fire from the torches and plays the fire sound effect
    /// </summary>
    private void EnableFireTorches()
    {
        torches.ForEach(torch => {
            torch.SetActive(true);
            audioSource.PlayOneShot(torchSoundEffect);
        });
    }
    /// <summary>
    /// Decrease the day light and increase the night light
    /// </summary>
    private IEnumerator DecreaseDayIncreaseNight()
    {
        yield return new WaitForSeconds(dayDuration);
        Light dayLight = day.GetComponent<Light>();
        Light nightLight = night.GetComponent<Light>();
        night.SetActive(true);
        StartCoroutine("IncreaseFog");
        while (true)
        {
            if (dayLight.intensity <= 0.0f)
            {
                dayLight.intensity = 0.0f;
                break;
            }
            dayLight.intensity -= dayLightMaxIntensity / 50.0f;
            nightLight.intensity += nightLightMaxIntensity / 50.0f;
            yield return new WaitForSeconds(0.1f);
        }
        day.SetActive(false);
        EnableFireTorches();
        StartCoroutine("DecreaseNightIncreaseDay");
    }
    /// <summary>
    /// Decrease the night light and increase the day lighty
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseNightIncreaseDay()
    {
        yield return new WaitForSeconds(nightDuration);
        Light dayLight = day.GetComponent<Light>();
        Light nightLight = night.GetComponent<Light>();
        day.SetActive(true);
        StartCoroutine("DecreaseFog");
        while (true)
        {
            if (dayLight.intensity >= dayLightMaxIntensity)
            {
                dayLight.intensity = dayLightMaxIntensity;
                break;
            }
            dayLight.intensity += dayLightMaxIntensity / 50.0f;
            nightLight.intensity -= nightLightMaxIntensity / 50.0f;
            yield return new WaitForSeconds(0.1f);
        }
        night.SetActive(false);
        DisableFireTorches();
        StartCoroutine("DecreaseDayIncreaseNight");
    }
    /// <summary>
    /// Decrease the fog density and disables the fog
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseFog()
    {
        while(true)
        {
            if (RenderSettings.fogDensity <= 0.0f) break;
            RenderSettings.fogDensity -= 0.001f;
            yield return new WaitForSeconds(0.1f);        
        }
        RenderSettings.fog = false;

    }
    /// <summary>
    /// Enables the fog and increse the fog density.
    /// </summary>
    /// <returns></returns>
    private IEnumerator IncreaseFog()
    {
        RenderSettings.fog = true;
        while (true)
        {
            if (RenderSettings.fogDensity >= maxFogDensity) break;
            RenderSettings.fogDensity += 0.001f;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public Environment GetEnvironment()
    {
        return ENVIRONMENT;
    }

}

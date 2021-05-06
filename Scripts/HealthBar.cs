using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    /// <summary>
    /// Set the slider value
    /// </summary>
    /// <param name="maxHealth">Value</param>
    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
    /// <summary>
    /// Set the slider value
    /// </summary>
    /// <param name="maxHealth">Value</param>
    public void SetHealth(float health)
    {
        slider.value = health;
    }
    #region Getters
    public float GetHealth()
    {
        return slider.value;
    }
    public float GetMaxHealth()
    {
        return slider.maxValue;
    }
    #endregion

}

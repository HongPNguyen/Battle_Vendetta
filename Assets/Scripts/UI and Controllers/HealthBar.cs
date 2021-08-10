using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    protected Slider slider;

    public void Start()
    {
        slider = this.GetComponent<Slider>();
    }

    public void SetHealth(float health)
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }

    public void SetMax(float health)
    {
        if (slider != null)
        {
            slider.maxValue = health;
            slider.value = health;
        }
    }
}

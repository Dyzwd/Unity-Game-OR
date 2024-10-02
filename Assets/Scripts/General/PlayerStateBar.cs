using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;

    public float healthDelayCounter;
    public float healthDelayTime;
    private void Awake()
    {
        healthDelayCounter=healthDelayTime;
    }

    public void OnHealthChange(float presentage)
    {
        healthImage.fillAmount = presentage;
    }
    private void Update()
    {
        CheckHealthDelayChange();
    }

    private void CheckHealthDelayChange()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayCounter=healthDelayCounter-Time.deltaTime;
            if(healthDelayCounter <= 0)
            {
                healthDelayImage.fillAmount-=Time.deltaTime;
                if (healthDelayImage.fillAmount <= healthImage.fillAmount)
                    healthDelayCounter = healthDelayTime;
            }
        }
        else
        {
            healthDelayImage.fillAmount = healthImage.fillAmount;
        }
    }
    public void OnPowerChange(float presentage)
    {
        powerImage.fillAmount = presentage;
    }
}

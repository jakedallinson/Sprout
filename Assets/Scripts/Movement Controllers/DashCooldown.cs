using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DashCooldown : MonoBehaviour
{
    public Slider slider;
    public Image sliderFill;

    private bool cooldownReached = false;
    private float coolDownSpeed = 0.25f;
    private float refillSpeed = 5.0f;

    public static bool dashUsed;
    public static bool dashAllowed;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 1.0f;
        dashUsed = false;
        setGreen();
    }

    void Update()
    {
        dashAllowed = !cooldownReached;
        if (cooldownReached)
        {
            // dash is not allowed, must cooldown
            if (slider.value < 1.0f)
            {
                slider.value += 1.0f / refillSpeed * Time.deltaTime;
            }
            else
            {
                cooldownReached = false;
                setGreen();
            }
        }
        else
        {
            // dash is allowed
            if (dashUsed)
            {
                // player dashed
                slider.value -= coolDownSpeed;
                dashUsed = false;
            }

            // check the cooldown level
            if (slider.value <= 0.01f)
            {
                cooldownReached = true;
                setRed();
            }
            else if (slider.value < 1.0f)
            {
                slider.value += 1.0f / refillSpeed * Time.deltaTime;
            }
        }
    }

    void setGreen()
    {
        sliderFill.color = new Color32(106,156,72,160);
    }

    void setRed()
    {
        sliderFill.color = new Color32(243,16,0,160);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushBar : MonoBehaviour
{
    public Slider PushSlider;

    public void SetStrength(int pushStrength)
    {
        PushSlider.value = pushStrength;
    }

    public void SetMaxStrength(int pushStrength)
    {
        PushSlider.maxValue = pushStrength;
        PushSlider.value = pushStrength;
    }
}

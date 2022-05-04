using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSliderOnEnable : MonoBehaviour
{
    public Slider statSlider;

    public void OnEnable()
    {
        Debug.Log("Enabled");
        statSlider.Select();
    }
}

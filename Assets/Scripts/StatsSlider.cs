using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsSlider : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sliderText;
    [SerializeField] string startString;
    [SerializeField] Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        updateSliderText();
    }

    public void updateSliderText()
    {
        sliderText.text = startString + ": " + slider.value.ToString();
    }
}

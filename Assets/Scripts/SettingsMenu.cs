using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider durationSlider;
    public Slider iterationSlider;
    public Toggle humansRemainingToggle;
    public TextMeshProUGUI durationValueText;
    public TextMeshProUGUI iterationValueText;

    // Update is called once per frame
    void Update()
    {
        durationValueText.text = durationSlider.value.ToString("F0") + "s";
        iterationValueText.text = iterationSlider.value.ToString("F0");
        Data.IterationDuration = (int)durationSlider.value;
        Data.IterationSteps = (int)iterationSlider.value;
        Data.StopWhenNoHumans = humansRemainingToggle.isOn;
    }
}

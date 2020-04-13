using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SimulationOverMenu : MonoBehaviour
{
    public TextMeshProUGUI aggressionText;
    public TextMeshProUGUI enduranceText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moveSpeedText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI thirstText;
    public TextMeshProUGUI survivalTimeText;
    void Start() {
        aggressionText.text = "Aggression:\t" + Data.Aggression.ToString("F2");
        enduranceText.text = "Endurance:\t\t" + Data.Endurance.ToString("F2");
        healthText.text = "Health:\t\t" + Data.Health.ToString("F2");
        moveSpeedText.text = "Move Speed:\t" + Data.MoveSpeed.ToString("F2");
        staminaText.text = "Stamina:\t" + Data.Stamina.ToString("F2");
        strengthText.text = "Strength:\t" + Data.Strength.ToString("F2");
        hungerText.text = "Hunger:\t" + Data.Hunger.ToString("F2");
        thirstText.text = "Thirst:\t" + Data.Thirst.ToString("F2");
        survivalTimeText.text = "Survival Time:\t" + Data.Age.ToString("F2");
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(0);
    }
}

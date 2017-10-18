using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepController : MonoBehaviour
{
    [SerializeField] private GameObject sleepUI;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Slider sleepSlider;
    [SerializeField] private Text sleepNumber;

    [SerializeField] private float hourlyRegen;
    [SerializeField] private DisableManager disableManager;

    void Start()
    {
        disableManager = GameObject.FindGameObjectWithTag("DisableController").GetComponent<DisableManager>();
    }

    public void EnableSleepUI()
    {
        sleepUI.SetActive(true);
        crosshair.SetActive(false);
        disableManager.DisablePlayer();
    }

    public void UpdateSlider()
    {
        sleepNumber.text = sleepSlider.value.ToString("0");
    }

    public void SleepButton(PlayerVitals playerVitals)
    {
        playerVitals.fatigueSlider.value = sleepSlider.value * hourlyRegen;
        playerVitals.fatigueMaxStamina = playerVitals.fatigueSlider.value;
        playerVitals.staminaSlider.value = playerVitals.normMaxStamina;
        sleepSlider.value = 1;
        crosshair.SetActive(true);
        disableManager.EnablePlayer();
        sleepUI.SetActive(false);
    }
}

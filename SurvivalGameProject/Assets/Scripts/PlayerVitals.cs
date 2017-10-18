using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerVitals : MonoBehaviour
{
    #region Slider Variables
    [Space(10)]
    [Header("Health Settings")]
    public Slider healthSlider;
    public int maxHealth;
    public int healthFallRate;

    [Space(10)]
    [Header("Thirst Settings")]
    public Slider thirstSlider;
    public int maxThirst;
    public int thirstFallRate;

    [Space(10)]
    [Header("Hunger Settings")]
    public Slider hungerSlider;
    public int maxHunger;
    public int hungerFallRate;

    [Space(10)]
    [Header("Stamina Settings")]
    public Slider staminaSlider;
    public int normMaxStamina;
    public float fatigueMaxStamina;
    public int staminaFallRate;
    public int staminaFallMultiplier;
    public int staminaRegainRate;
    public int staminaRegainMultiplier;

    [Space(10)]
    [Header("Fatigue Settings")]
    public Slider fatigueSlider;
    public int normFatigue;
    public int maxFatigue;
    public int fatigueFallRate;

    [Space(10)]
    [Header("Temperature Settings")]
    public float freezingTemp;
    public float currentTemp;
    public float normalTemp;
    public float heatTemp;
    public Text tempNumber;
    public Image tempBG;
    #endregion

    private CharacterController characterController;
    private FirstPersonController playerController;
    
    void Start()
    {
        #region Slider Default Setup
        fatigueSlider.maxValue = maxFatigue;
        fatigueSlider.value = maxFatigue;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        thirstSlider.maxValue = maxThirst;
        thirstSlider.value = maxThirst;

        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = maxHunger;

        staminaSlider.maxValue = normMaxStamina;
        staminaSlider.value = normMaxStamina;
        #endregion

        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<FirstPersonController>();
    }

    void FixedUpdate()
    {
    }

    void Update()
    {
        Debug.Log("Update:" + Time.deltaTime);

        #region Vital Controllers
        
        #region Fatigue Controller
        float fatigueMultiplier = normFatigue / (fatigueSlider.value + 1);
        if (fatigueMultiplier >= 10)
        {
            fatigueMultiplier = 4;
        }

        if (fatigueSlider.value >= 0)
        {
            fatigueSlider.value -= Time.deltaTime * fatigueFallRate;
        }

        else if (fatigueSlider.value <= 0)
        {
            fatigueSlider.value = 0;
        }

        else if (fatigueSlider.value >= maxFatigue)
        {
            fatigueSlider.value = maxFatigue;
        }
        #endregion

        #region Temperature Controller
        if (currentTemp <= freezingTemp){
            tempBG.color = Color.blue;
            UpdateTemp();
        }

        else if(currentTemp >= heatTemp - 0.1){
            tempBG.color = Color.red;
            UpdateTemp();
        }

        else
        {
            tempBG.color = Color.green;
            UpdateTemp();
        }
        #endregion

        #region Health Controller
        if(hungerSlider.value <= 0 && thirstSlider.value <= 0)
        {
            healthSlider.value -= Time.deltaTime / healthFallRate * 3;
        }

        else if (hungerSlider.value <= 0 && thirstSlider.value <= 0 && (currentTemp <= freezingTemp || currentTemp >= heatTemp))
        {
            healthSlider.value -= Time.deltaTime / healthFallRate * 5;
        }

        else if (hungerSlider.value <= 0 && thirstSlider.value <= 0 && (currentTemp <= freezingTemp || currentTemp >= heatTemp) && fatigueSlider.value <= 0)
        {
            healthSlider.value -= Time.deltaTime / healthFallRate * 10;
        }

        else if ((currentTemp <= freezingTemp || currentTemp >= heatTemp) && staminaSlider.value <= 0)
        {
            healthSlider.value -= Time.deltaTime / healthFallRate * 20;
        }

        else if (hungerSlider.value <= 0 || thirstSlider.value <= 0 || currentTemp <= freezingTemp || currentTemp >= heatTemp || fatigueSlider.value <=0 || staminaSlider.value <=0)
        {
            healthSlider.value -= Time.deltaTime / healthFallRate;
        }
        
        if(healthSlider.value <= 0)
        {
            CharacterDeath();
        }
        #endregion

        #region Hunger Controller
        if (hungerSlider.value >= 0)
        {
            hungerSlider.value -= Time.deltaTime * hungerFallRate * fatigueMultiplier;
        }

        else if(hungerSlider.value <= 0)
        {
            hungerSlider.value = 0;
        }

        else if(hungerSlider.value >= maxHunger)
        {
            hungerSlider.value = maxHunger;
        }
        #endregion 

        #region Thirst Controller
        if (thirstSlider.value >= 0)
        {
            thirstSlider.value -= Time.deltaTime * thirstFallRate * fatigueMultiplier;
        }

        else if (thirstSlider.value <= 0)
        {
            thirstSlider.value = 0;
        }

        else if (thirstSlider.value >= maxThirst)
        {
            thirstSlider.value = maxThirst;
        }
        #endregion 

        #region Stamina Controller
        if(characterController.velocity.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            staminaSlider.value -= Time.deltaTime / staminaFallRate * staminaFallMultiplier * fatigueMultiplier;

            if(staminaSlider.value > 0)
            {
                currentTemp += Time.deltaTime / 5;
            }
        }

        else
        {
            staminaSlider.value += Time.deltaTime / staminaRegainRate * staminaRegainMultiplier / (fatigueMultiplier / 2);

            if(currentTemp >= normalTemp)
            {
                currentTemp -= Time.deltaTime / 10;
            }
        }

        if(staminaSlider.value <= 0)
        {
            staminaSlider.value = 0;
            playerController.m_RunSpeed = playerController.m_ExhaustedSpeed;
        }

        else if(staminaSlider.value >= 0)
        {
            playerController.m_RunSpeed = playerController.m_RunSpeedNorm;
        }
        #endregion

        #endregion
    }

    void UpdateTemp()
    {
        tempNumber.text = currentTemp.ToString("00.0");
    }

    void CharacterDeath()
    {
        //DO SOMETHING HERE!
    }
}

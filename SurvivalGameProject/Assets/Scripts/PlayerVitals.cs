using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerVitals : MonoBehaviour
{
    #region Settings
    [Space(10)]
    [Header("Health Settings")]
    public Slider healthSlider;
    public int maxHealth;
    public int healthFallRate;
    public int effectMultiplier;

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
    public float normalTemp;
    public float heatTemp;
    public Text tempNumber;
    public Image tempBG;

    [Space(10)]
    [Header("References")]
    public CharacterController characterController;
    public FirstPersonController playerController;
    #endregion

    #region Debugging
    [Space(10)]
    [Header("Debugging")]
    public bool isTired;
    public bool isStarving;
    public bool isDehydrated;
    public bool isOverheating;
    public bool isFreezing;
    public bool isExhausted;
    public bool isRunning;
    public bool isStandingStill;
    public bool isWalking;
    public bool isExhaustedRunning;

    [Space(10)]
    public float currentTemp;
    public float currentFatigue;
    public float currentHealth;
    public float currentHunger;
    public float currentThirst;
    public float currentStamina;
    public string currentMovementSpeed;

    [Space(10)]
    public bool allowDeath;
    #endregion

    #region Controllers
    //public CharacterController characterController;
    //public FirstPersonController playerController;
    #endregion

    void Start()
    {
        #region Slider Default Setup
        fatigueSlider.maxValue = maxFatigue;
        fatigueSlider.value = maxFatigue;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        effectMultiplier = 0;

        thirstSlider.maxValue = maxThirst;
        thirstSlider.value = maxThirst;

        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = maxHunger;

        staminaSlider.maxValue = normMaxStamina;
        staminaSlider.value = normMaxStamina;
        #endregion

        #region Debugging Values
        currentFatigue = fatigueSlider.value;
        currentHealth = healthSlider.value;
        currentHunger = hungerSlider.value;
        currentThirst = thirstSlider.value;
        currentStamina = staminaSlider.value;
        #endregion

        #region Controllers
        //characterController = GetComponent<CharacterController>();
        //playerController = GetComponent<FirstPersonController>();
        #endregion Controllers

        currentMovementSpeed = characterController.velocity.magnitude.ToString("0");
    }

    void FixedUpdate()
    {
        #region Vital Controllers

        #region Fatigue Controller
        float fatigueMultiplier = normFatigue / (fatigueSlider.value + 1);
        if (fatigueMultiplier >= 4)
        {
            fatigueMultiplier = 4;
        }

        if (fatigueSlider.value > 0)
        {
            fatigueSlider.value -= Time.deltaTime * fatigueFallRate;
            if (isTired)
            {
                isTired = false;
                effectMultiplier--;
            }
        }

        else if (fatigueSlider.value <= 0)
        {
            fatigueSlider.value = 0;
            if (!isTired)
            {
                isTired = true;
                effectMultiplier++;
            }
        }

        else if (fatigueSlider.value >= maxFatigue)
        {
            fatigueSlider.value = maxFatigue;
        }

        currentFatigue = fatigueSlider.value;
        #endregion

        #region Temperature Controller
        if (currentTemp <= freezingTemp)
        {
            tempBG.color = Color.blue;

            if (!isFreezing)
            {
                isFreezing = true;
                effectMultiplier++;
            }
        }

        else if (currentTemp >= heatTemp - 0.1)
        {
            tempBG.color = Color.red;

            if (!isOverheating)
            {
                isOverheating = true;
                effectMultiplier++;
            }
        }

        else
        {
            tempBG.color = Color.green;

            if (isFreezing || isOverheating)
            {
                isOverheating = false;
                isFreezing = false;
                effectMultiplier--;
            }
        }
        #endregion

        #region Health Controller
        healthSlider.value -= (effectMultiplier * effectMultiplier) * Time.deltaTime;

        if (healthSlider.value <= 0 && allowDeath)
        {
            CharacterDeath();
        }

        currentHealth = healthSlider.value;
        #endregion

        #region Hunger Controller
        if (hungerSlider.value > 0)
        {
            hungerSlider.value -= Time.deltaTime * hungerFallRate * fatigueMultiplier;
            if (isStarving)
            {
                isStarving = false;
                effectMultiplier--;
            }
        }

        else if (hungerSlider.value <= 0)
        {
            hungerSlider.value = 0;
            if (!isStarving)
            {
                isStarving = true;
                effectMultiplier++;
            }
        }

        else if (hungerSlider.value >= maxHunger)
        {
            hungerSlider.value = maxHunger;
        }
        currentHunger = hungerSlider.value;
        #endregion 

        #region Thirst Controller
        if (thirstSlider.value > 0)
        {
            thirstSlider.value -= Time.deltaTime * thirstFallRate * fatigueMultiplier;

            if (isDehydrated)
            {
                isDehydrated = false;
                effectMultiplier--;
            }
        }

        else if (thirstSlider.value <= 0)
        {
            thirstSlider.value = 0;
            if (!isDehydrated)
            {
                isDehydrated = true;
                effectMultiplier++;
            }
        }

        else if (thirstSlider.value >= maxThirst)
        {
            thirstSlider.value = maxThirst;
        }
        currentThirst = thirstSlider.value;
        #endregion 

        #region Stamina Controller
        #region IsOperators
        if (staminaSlider.value > 0)
        {
            if (isExhausted)
            {
                isExhausted = false;
                effectMultiplier--;
            }
            playerController.m_RunSpeed = playerController.m_RunSpeedNorm;
        }

        else if (staminaSlider.value <= 0)
        {
            if (!isExhausted)
            {
                isExhausted = true;
                effectMultiplier++;
            }
            staminaSlider.value = 0;
            playerController.m_RunSpeed = playerController.m_ExhaustedSpeed;
        }

        if (characterController.velocity.magnitude > 0 && Input.GetKey(KeyCode.LeftShift) && isExhausted)
        {
            isRunning = false;
            isStandingStill = false;
            isWalking = false;
            isExhaustedRunning = true;
        }

        else if (characterController.velocity.magnitude > 0 && Input.GetKey(KeyCode.LeftShift) && !isExhausted)
        {
            isRunning = true;
            isStandingStill = false;
            isWalking = false;
            isExhaustedRunning = false;
        }

        else if (characterController.velocity.magnitude > 0)
        {
            isRunning = false;
            isStandingStill = false;
            isWalking = true;
            isExhaustedRunning = false;
        }

        else
        {
            isWalking = false;
            isRunning = false;
            isStandingStill = true;
            isExhaustedRunning = false;
        }
        #endregion

        if (isRunning || isExhaustedRunning)
        {
            staminaSlider.value -= Time.deltaTime / staminaFallRate * staminaFallMultiplier * (fatigueMultiplier / 2);
            currentTemp += Time.deltaTime / 30;
        }

        else if (isWalking)
        {
            staminaSlider.value -= Time.deltaTime / staminaRegainRate * staminaRegainMultiplier * (fatigueMultiplier / 4);
            currentTemp += Time.deltaTime / 60;
        }

        else if (isStandingStill)
        {
            staminaSlider.value += Time.deltaTime / staminaRegainRate * staminaRegainMultiplier / (fatigueMultiplier / 2);

            if (currentTemp >= normalTemp)
            {
                currentTemp -= Time.deltaTime / 40;
            }
        }
        currentStamina = staminaSlider.value;
        #endregion

        #endregion
    }

    void Update()
    {
        currentMovementSpeed = characterController.velocity.magnitude.ToString("0");
        tempNumber.text = currentTemp.ToString("00.0");
    }

    void CharacterDeath()
    {
        //DO SOMETHING HERE!
    }
}

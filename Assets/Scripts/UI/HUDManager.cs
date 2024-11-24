using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Slider hpSlider;
    public Image hpFill;
    public Gradient hpGradient;
    public Slider staminaSlider;

    private void OnEnable()
    {
        //subscribing to actions
        Actions.UpdatePlayerHealthBar += UpdateHealthBar;
        Actions.UpdatePlayerStaminaBar += UpdateStaminaBar;
    }

    private void OnDisable()
    {
        //unsubscribing to actions
        Actions.UpdatePlayerHealthBar -= UpdateHealthBar;
        Actions.UpdatePlayerStaminaBar -= UpdateStaminaBar;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(PlayerController player)
    {
        hpSlider.maxValue = player.maxHp;
        hpSlider.value = player.currentHp;
        hpFill.color = hpGradient.Evaluate(hpSlider.normalizedValue);
    }
    public void UpdateStaminaBar(PlayerController player)
    {
        staminaSlider.maxValue = player.maxStamina;
        staminaSlider.value = player.currentStamina;
    }

}

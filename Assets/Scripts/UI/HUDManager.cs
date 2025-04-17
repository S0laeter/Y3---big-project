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
    public Slider energySlider;
    public Slider heatSlider;

    public Slider bossHpSlider;
    public Slider bossArmorSlider;

    private void OnEnable()
    {
        //subscribing to actions
        Actions.UpdatePlayerHealthBar += UpdateHealthBar;
        Actions.UpdatePlayerStaminaBar += UpdateStaminaBar;
        Actions.UpdatePlayerEnergyBar += UpdateEnergyBar;
        Actions.UpdatePlayerHeatBar += UpdateHeatBar;

        Actions.UpdateBossHealthBar += UpdateBossHealthBar;
        Actions.UpdateBossArmorBar += UpdateBossArmorBar;
    }

    private void OnDisable()
    {
        //unsubscribing to actions
        Actions.UpdatePlayerHealthBar -= UpdateHealthBar;
        Actions.UpdatePlayerStaminaBar -= UpdateStaminaBar;
        Actions.UpdatePlayerEnergyBar -= UpdateEnergyBar;
        Actions.UpdatePlayerHeatBar -= UpdateHeatBar;

        Actions.UpdateBossHealthBar -= UpdateBossHealthBar;
        Actions.UpdateBossArmorBar -= UpdateBossArmorBar;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthBar(PlayerBehavior player)
    {
        hpSlider.maxValue = player.maxHp;
        hpSlider.value = player.currentHp;
        hpFill.color = hpGradient.Evaluate(hpSlider.normalizedValue);
    }
    public void UpdateStaminaBar(PlayerBehavior player)
    {
        staminaSlider.maxValue = player.maxStamina;
        staminaSlider.value = player.currentStamina;
    }
    public void UpdateEnergyBar(PlayerMechanics player)
    {
        energySlider.maxValue = player.maxEnergy;
        energySlider.value = player.currentEnergy;
    }
    public void UpdateHeatBar(PlayerMechanics player)
    {
        heatSlider.maxValue = player.maxHeat;
        heatSlider.value = player.currentHeat;
    }

    public void UpdateBossHealthBar(EnemyBehavior boss)
    {
        bossHpSlider.maxValue = boss.maxHp;
        bossHpSlider.value = boss.currentHp;
    }
    public void UpdateBossArmorBar(EnemyBehavior boss)
    {
        bossArmorSlider.maxValue = boss.maxArmor;
        bossArmorSlider.value = boss.currentArmor;
    }

}

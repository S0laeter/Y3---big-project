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
    public Image heat1;
    public Image heat2;
    public Image heat3;

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
        switch (player.currentHeat)
        {
            case 0:
                heat1.enabled = false;
                heat2.enabled = false;
                heat3.enabled = false;
                break;
            case 1:
                heat1.enabled = true;
                heat2.enabled = false;
                heat3.enabled = false;
                break;
            case 2:
                heat1.enabled = true;
                heat2.enabled = true;
                heat3.enabled = false;
                break;
            case 3:
                heat1.enabled = true;
                heat2.enabled = true;
                heat3.enabled = true;
                break;
            default:
                break;
        }
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

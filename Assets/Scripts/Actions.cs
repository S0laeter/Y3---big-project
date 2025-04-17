using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static Action<PlayerBehavior> UpdatePlayerHealthBar;
    public static Action<PlayerBehavior> UpdatePlayerStaminaBar;
    public static Action<PlayerMechanics> UpdatePlayerEnergyBar;
    public static Action<PlayerMechanics> UpdatePlayerHeatBar;

    public static Action<EnemyBehavior> UpdateBossHealthBar;
    public static Action<EnemyBehavior> UpdateBossArmorBar;

    public static Action<float> GainEnergyOnHit;

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    private PlayerBehavior player;
    private StateMachine stateMachine;

    public float baseAtk;
    public float currentAtk;

    public float currentEnergy;
    public float maxEnergy;
    public int currentHeat;
    public int maxHeat;

    private void OnEnable()
    {
        //subscribing to actions
        Actions.GainEnergyOnHit += GainEnergy;
    }

    private void OnDisable()
    {
        //unsubscribing to actions
        Actions.GainEnergyOnHit -= GainEnergy;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerBehavior>();
        stateMachine = GetComponent<StateMachine>();

        currentEnergy = 0;
        currentHeat = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }




    //can call this from somewhere else
    //gaining and spending use this same function, less stuff to keep track of
    public void GainEnergy(float energyToGain)
    {
        currentEnergy += Mathf.Clamp(energyToGain, 0f, maxEnergy);
    }
    public void GainHeat(int heatToGain)
    {
        currentHeat += heatToGain;

        //well, i couldnt find a clamp for int..
        if (currentHeat > maxHeat)
            currentHeat = maxHeat;
        else if (currentEnergy < 0)
            currentEnergy = 0;
    }





    public void SpawnHitbox(string whichAttack)
    {
        //spawn it here first, and get the script
        GameObject hitboxObject = ObjectPool.instance.SpawnObject("hitbox", transform.position, transform.rotation);
        HitboxBehavior hitbox = hitboxObject.GetComponent<HitboxBehavior>();
        hitbox.targetTag = "Enemy";

        //adjust the object's paramaters, a scuffed way to specify individual attack damage
        switch (whichAttack)
        {
            case "basic 1":
                hitbox.damage = currentAtk * 10;
                hitbox.armorDamage = 10;
                hitbox.range = 2;
                hitbox.energyOnHit = 5;
                break;
            case "basic 2":
                hitbox.damage = currentAtk * 8;
                hitbox.armorDamage = 8;
                hitbox.range = 2;
                hitbox.energyOnHit = 5;
                break;
            case "basic 3":
                hitbox.damage = currentAtk * 12;
                hitbox.armorDamage = 12;
                hitbox.range = 2;
                hitbox.energyOnHit = 7;
                break;
            case "basic 4":
                hitbox.damage = currentAtk * 8;
                hitbox.armorDamage = 8;
                hitbox.range = 2;
                hitbox.energyOnHit = 5;
                break;
            case "basic 4.1":
                hitbox.damage = currentAtk * 8;
                hitbox.armorDamage = 8;
                hitbox.range = 2;
                hitbox.energyOnHit = 5;
                break;
            case "basic 5":
                hitbox.damage = currentAtk * 6;
                hitbox.armorDamage = 6;
                hitbox.range = 2;
                hitbox.energyOnHit = 5;
                break;
            case "basic 5.1":
                hitbox.damage = currentAtk * 10;
                hitbox.armorDamage = 5;
                hitbox.range = 2;
                hitbox.energyOnHit = 10;
                break;
            case "heavy":
                if (currentEnergy >= maxEnergy)
                {
                    //multi hit
                    GameObject multiHitboxObject = ObjectPool.instance.SpawnObject("multiHitbox", transform.position, transform.rotation);
                    MultiHitbox multiHitbox = multiHitboxObject.GetComponent<MultiHitbox>();
                    multiHitbox.spawnAmount = 3;
                    multiHitbox.spawnInterval = 0.2f;
                    multiHitbox.targetTag = "Enemy";

                    multiHitbox.damage = 8;
                    multiHitbox.armorDamage = 8;
                    multiHitbox.range = 5;
                    
                    //spend energy and gain heat, doesnt matter if hit
                    multiHitbox.energyOnHit = -maxEnergy;
                    GainHeat(1);
                }
                else
                {
                    hitbox.damage = currentAtk * 10;
                    hitbox.armorDamage = 15;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;
                }
                break;
            case "skill 1":
                if (currentHeat > 0)
                {
                    hitbox.damage = currentAtk * 15;
                    hitbox.armorDamage = 12;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;
                }
                else
                {
                    hitbox.damage = currentAtk * 6;
                    hitbox.armorDamage = 6;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;
                }
                break;
            case "skill 1.1":
                if (currentHeat > 0)
                {
                    hitbox.damage = currentAtk * 15;
                    hitbox.armorDamage = 12;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;

                    //spend heat
                    GainHeat(-1);
                }
                else
                {
                    hitbox.damage = currentAtk * 6;
                    hitbox.armorDamage = 6;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;
                }
                break;
            case "skill 2":
                if (currentHeat > 0)
                {
                    hitbox.damage = currentAtk * 15;
                    hitbox.armorDamage = 10;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;
                }
                else
                {
                    hitbox.damage = currentAtk * 6;
                    hitbox.armorDamage = 6;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;
                }
                break;
            case "skill 2.1":
                if (currentHeat > 0)
                {
                    hitbox.damage = currentAtk * 22;
                    hitbox.armorDamage = 20;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;

                    //spend heat
                    GainHeat(-1);
                }
                else
                {
                    hitbox.damage = currentAtk * 10;
                    hitbox.armorDamage = 10;
                    hitbox.range = 5;
                    hitbox.energyOnHit = 0;
                }
                break;
            default:
                break;
        }

    }



}

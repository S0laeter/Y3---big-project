using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    private PlayerBehavior player;
    private StateMachine stateMachine;
    public Transform spawnTransform;

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

        currentAtk = baseAtk;

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
        //spawn it here first, parent it to this, then get the script
        GameObject hitboxObject = ObjectPool.instance.SpawnObject("hitbox", spawnTransform.position, spawnTransform.rotation);
        hitboxObject.transform.SetParent(this.transform);
        HitboxBehavior hitbox = hitboxObject.GetComponent<HitboxBehavior>();
        hitbox.targetTag = "Enemy";

        //adjust the object's paramaters, a scuffed way to specify individual attack damage
        switch (whichAttack)
        {
            case "basic 1":
                hitbox.damage = currentAtk * 10;
                hitbox.armorDamage = 8;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 5;
                break;
            case "basic 2":
                hitbox.damage = currentAtk * 8;
                hitbox.armorDamage = 7;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 5;
                break;
            case "basic 3":
                hitbox.damage = currentAtk * 12;
                hitbox.armorDamage = 10;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 7;
                break;
            case "basic 4":
                hitbox.damage = currentAtk * 8;
                hitbox.armorDamage = 10;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 5;
                break;
            case "basic 4.1":
                hitbox.damage = currentAtk * 8;
                hitbox.armorDamage = 8;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 5;
                break;
            case "basic 5":
                hitbox.damage = currentAtk * 6;
                hitbox.armorDamage = 10;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 5;
                break;
            case "basic 5.1":
                hitbox.damage = currentAtk * 10;
                hitbox.armorDamage = 18;
                hitbox.range = 2;
                hitbox.energyOnHit = 10;
                break;
            case "air 1":
                hitbox.damage = currentAtk * 13;
                hitbox.armorDamage = 10;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 7;
                break;
            case "air 2":
                hitbox.damage = currentAtk * 10;
                hitbox.armorDamage = 10;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 7;
                break;
            case "plunge":
                hitbox.damage = currentAtk * 20;
                hitbox.armorDamage = 20;
                hitbox.range = 3;
                hitbox.energyOnHit = 10;
                break;
            case "heavy":
                if (currentEnergy >= maxEnergy)
                {
                    //multi hit
                    GameObject multiHitboxObject = ObjectPool.instance.SpawnObject("multiHitbox", spawnTransform.position, spawnTransform.rotation);
                    MultiHitbox multiHitbox = multiHitboxObject.GetComponent<MultiHitbox>();
                    multiHitbox.spawnAmount = 3;
                    multiHitbox.spawnInterval = 0.2f;
                    multiHitbox.targetTag = "Enemy";

                    multiHitbox.damage = 8;
                    multiHitbox.armorDamage = 8;
                    multiHitbox.range = 3;
                    
                    //spend energy and gain heat, doesnt matter if hit
                    multiHitbox.energyOnHit = -currentEnergy;
                    GainHeat(1);
                }
                else
                {
                    hitbox.damage = currentAtk * 10;
                    hitbox.armorDamage = 15;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                break;
            case "skill 1":
                if (currentHeat > 0)
                {
                    hitbox.damage = currentAtk * 18;
                    hitbox.armorDamage = 16;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                else
                {
                    hitbox.damage = currentAtk * 10;
                    hitbox.armorDamage = 8;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                break;
            case "skill 1.1":
                if (currentHeat > 0)
                {
                    hitbox.damage = currentAtk * 16;
                    hitbox.armorDamage = 14;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                else
                {
                    hitbox.damage = currentAtk * 10;
                    hitbox.armorDamage = 8;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                break;
            case "skill 2":
                if (currentHeat > 0)
                {
                    hitbox.damage = currentAtk * 16;
                    hitbox.armorDamage = 14;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                else
                {
                    hitbox.damage = currentAtk * 10;
                    hitbox.armorDamage = 8;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                break;
            case "skill 2.1":
                if (currentHeat > 0)
                {
                    hitbox.damage = currentAtk * 20;
                    hitbox.armorDamage = 22;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                else
                {
                    hitbox.damage = currentAtk * 13;
                    hitbox.armorDamage = 10;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;
                }
                break;
            default:
                break;
        }

    }



}

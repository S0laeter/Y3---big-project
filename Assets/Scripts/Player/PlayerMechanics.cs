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
                hitbox.damage = currentAtk * 10f;
                hitbox.armorDamage = currentAtk * 10f;
                hitbox.range = 2f;
                break;
            case "basic 2":
                //get the hitbox object, then set the damage of it
                break;
            default:
                break;
        }

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    private PlayerBehavior player;
    private StateMachine stateMachine;

    public float baseAttack;
    public float currentAttack;

    public float currentEnergy;
    public float maxEnergy;
    public int currentHeat;
    public int maxHeat;

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

    public void SpawnHitbox(string whichAttack)
    {
        //spawn it here first, and get the script
        GameObject hitboxObject = ObjectPool.instance.SpawnObject("hitbox", transform.position, transform.rotation);
        HitboxBehavior hitbox = hitboxObject.GetComponent<HitboxBehavior>();
        hitbox.targetTag = "enemy";

        //adjust the object's paramaters, a scuffed way to specify individual attack damage
        switch (whichAttack)
        {
            case "basic 1":
                hitbox.damage = currentAttack * 10f;
                hitbox.armorDamage = currentAttack * 10f;
                break;
            case "basic 2":
                //get the hitbox object, then set the damage of it
                break;
            default:
                break;
        }

    }



}

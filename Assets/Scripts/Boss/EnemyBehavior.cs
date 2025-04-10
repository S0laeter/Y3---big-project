using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private StateMachine stateMachine;
    public Animator anim;
    public Transform spawnTransform;

    public NavMeshAgent agent;
    public Transform playerTransform;

    public float maxHp;
    public float currentHp;
    public float maxArmor;
    public float currentArmor;
    public float baseAtk;
    public float currentAtk;

    public int currentPhase;

    //atk conditions
    public bool inRange;
    public bool outOfRange;

    private float testTime;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        currentHp = maxHp;
        currentArmor = maxArmor;
        currentAtk = baseAtk;

        currentPhase = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //hp check
        if (currentHp <= 0)
        {
            Die();
        }
        //if hp is lower than a certain point, go phase 2
        else if (currentHp < maxHp * 0.6f)
        {
            currentPhase = 2;
        }

        //armor regen
        if (currentArmor <= maxArmor && stateMachine.currentState.GetType() != typeof(EnemyStaggeredState))
        {
            currentArmor += Mathf.Clamp(5f * Time.deltaTime, 0f, maxArmor);
        }


        //calculate distance from player
        float distanceFromPlayer = Vector3.Distance(this.transform.position, playerTransform.position);
        //choose actions based on distance
        //navmesh's stopping distance is buggy as fuck ngl..
        if (distanceFromPlayer > agent.stoppingDistance)
        {
            outOfRange = true;
            inRange = false;
        }
        else if (distanceFromPlayer <= agent.stoppingDistance)
        {
            inRange = true;
            outOfRange = false;
        }




        testTime += Time.deltaTime;
        while (testTime >= 0.2f)
        {
            SpawnHitbox("fire bullet");
            testTime -= 0.2f;
        }




    }







    public void RotateToPlayer(float turnSmoothTime)
    {
        //get direction of player, sometimes its the opposite btw..
        Vector3 relativePosition = transform.position - playerTransform.position;
        //this is so the character doesnt look up or down, only straight forward
        relativePosition.y = 0f;
        //rotate to player, smoothly (bigger the faster, weird)
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSmoothTime);
    }
    
    
    




    
    
    public void TakeDamage(float damage, float armorDamage)
    {
        //if already dead, nvm
        if (stateMachine.currentState.GetType() == typeof(EnemyDeathState))
            return;

        currentHp -= Mathf.Clamp(damage, 0f, maxHp);

        //if not already staggered, reduce armor
        if (stateMachine.currentState.GetType() != typeof(EnemyStaggeredState))
        {
            currentArmor -= Mathf.Clamp(armorDamage, 0f, maxArmor);
            //if out of armor, stagger
            if (currentArmor <= 0f)
                stateMachine.SetNextState(new EnemyStaggeredState());
        }
        
    }
    public void Die()
    {
        stateMachine.SetNextState(new EnemyDeathState());
    }









    public void SpawnHitbox(string whichAttack)
    {
        //spawn it here first, parent it to the spawn point just incase we need to move it, then get the script
        GameObject hitboxObject = ObjectPool.instance.SpawnObject("hitbox", spawnTransform.position, spawnTransform.rotation);
        hitboxObject.transform.SetParent(spawnTransform);
        HitboxBehavior hitbox = hitboxObject.GetComponent<HitboxBehavior>();
        hitbox.targetTag = "Player";

        switch (whichAttack)
        {
            case "combo 1":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 1.5f;
                hitbox.type = 0;
                break;
            case "combo 1.1":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "combo 1 follow up":
                //dont parent it to the boss
                hitboxObject.transform.SetParent(null);
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "combo 2":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 0;
                break;
            case "combo 2.1":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "combo 2 follow up":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 0;
                break;
            case "combo 2 follow up.1":
                //dont parent it to the boss
                hitboxObject.transform.SetParent(null);
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "combo back":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "combo overhead":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "spin":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "dash punch":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "dive punch":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "slam":
                //dont parent it to the boss
                hitboxObject.transform.SetParent(null);
                hitbox.damage = currentAtk * 10;
                hitbox.range = 3f;
                hitbox.type = 1;
                break;
            case "fire bullet":
                //spawn bullets
                GameObject bulletObject = ObjectPool.instance.SpawnObject("bullet", spawnTransform.position, spawnTransform.rotation);
                //change speed with that float at the end
                bulletObject.GetComponent<Rigidbody>().AddForce(-transform.forward * 1700f);
                HitboxBehavior bulletHitbox = bulletObject.GetComponent<HitboxBehavior>();
                bulletHitbox.targetTag = "Player";
                bulletHitbox.damage = currentAtk * 10;
                bulletHitbox.range = 2f;
                bulletHitbox.type = 0;
                break;
            default:
                break;

        }

    }

}

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

        currentPhase = 1;
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

    }


    public void SpawnHitbox(string whichAttack, string type)
    {
        //spawn it here first, parent it to this, then get the script
        GameObject hitboxObject = ObjectPool.instance.SpawnObject("hitbox", spawnTransform.position, spawnTransform.rotation);
        hitboxObject.transform.SetParent(this.transform);
        HitboxBehavior hitbox = hitboxObject.GetComponent<HitboxBehavior>();
        hitbox.targetTag = "Player";



    }


}

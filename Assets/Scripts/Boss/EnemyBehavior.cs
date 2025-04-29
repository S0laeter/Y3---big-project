using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
    public BossAudio bossAudio;
    private StateMachine stateMachine;
    public Animator anim;

    public Transform spawnTransform;
    public Transform fxSpawnTransform;

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
        bossAudio = GetComponent<BossAudio>();
        stateMachine = GetComponent<StateMachine>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        currentHp = maxHp;
        currentArmor = maxArmor;
        currentAtk = baseAtk;
        Actions.UpdateBossHealthBar(this);
        Actions.UpdateBossArmorBar(this);

        //set this to 1
        currentPhase = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //hp check
        if (currentHp <= 0 && stateMachine.currentState.GetType() != typeof(EnemyDeathState))
        {
            stateMachine.SetNextState(new EnemyDeathState());
        }

        //armor regen
        if (currentArmor <= maxArmor && stateMachine.currentState.GetType() != typeof(EnemyStaggeredState))
        {
            currentArmor += Mathf.Clamp(3f * Time.deltaTime, 0f, maxArmor);
            Actions.UpdateBossArmorBar(this);
        }





        //calculate distance from player
        float distanceFromPlayer = Vector3.Distance(this.transform.position, playerTransform.position);
        //choose actions based on distance
        //navmesh's stopping distance is buggy as fuck ngl..
        if (distanceFromPlayer > agent.stoppingDistance + 4)
        {
            outOfRange = true;
            inRange = false;
        }
        else if (distanceFromPlayer <= agent.stoppingDistance + 4)
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

        //rotate to player, smoothly (bigger the faster)
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSmoothTime);
    }
    
    
    




    
    
    public void TakeDamage(float damage, float armorDamage)
    {
        //if already dead, nvm
        if (stateMachine.currentState.GetType() == typeof(EnemyDeathState))
            return;

        //deduct hp, update hp bar
        currentHp -= Mathf.Clamp(damage, 0f, maxHp);
        Actions.UpdateBossHealthBar(this);

        //audio (visual fx is already on the hitbox)
        bossAudio.PlayAudioClip("bossHit");

        //if not already staggered, reduce armor
        if (stateMachine.currentState.GetType() != typeof(EnemyStaggeredState))
        {
            currentArmor -= Mathf.Clamp(armorDamage, 0f, maxArmor);

            Actions.UpdateBossArmorBar(this);

            //if out of armor, stagger
            if (currentArmor <= 0f)
            {
                currentArmor = 0f;
                stateMachine.SetNextState(new EnemyStaggeredState());
            }
            else if (currentArmor > maxArmor)
            {
                currentArmor = maxArmor;
            }
        }
        
    }



    //hit effects
    public void HitEffect(Vector3 position)
    {
        //get direction of player, sometimes its the opposite btw..
        Vector3 relativePosition = transform.position - playerTransform.position;
        relativePosition.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);

        //spawn in a direction
        GameObject sparkObj = ObjectPool.instance.SpawnObject("sparkEffect", position, rotation);

        //make sure it plays
        sparkObj.GetComponent<ParticleSystem>().Play(true);

        //parent it to the player
        sparkObj.transform.SetParent(this.transform);

    }
    //muzzle effects
    public void MuzzleEffect()
    {
        //spawn in a direction
        GameObject muzzleFlashObj = ObjectPool.instance.SpawnObject("muzzleEffect", spawnTransform.position, spawnTransform.rotation);
        muzzleFlashObj.transform.Rotate(0, 0, 90);

        //make sure it plays
        muzzleFlashObj.GetComponent<ParticleSystem>().Play(true);

        //parent it to the player
        muzzleFlashObj.transform.SetParent(this.transform);

    }
    public void ExplosionEffect()
    {
        //spawn in a direction
        GameObject explosionObj = ObjectPool.instance.SpawnObject("bigExplosionEffect", spawnTransform.position, spawnTransform.rotation);

        //make sure it plays
        explosionObj.GetComponent<ParticleSystem>().Play(true);

    }
    public void SlamEffect(float size)
    {
        //spawn in a direction
        GameObject slamEffect = ObjectPool.instance.SpawnObject("slamEffect", spawnTransform.position, spawnTransform.rotation);
        slamEffect.transform.SetParent(this.transform);
        slamEffect.transform.localScale = new Vector3(size, size, size);

        //make sure it plays
        slamEffect.GetComponent<ParticleSystem>().Play(true);

    }
    public void GunshotEffect()
    {
        //spawn in a direction
        GameObject shotEffect = ObjectPool.instance.SpawnObject("gunshotEffect", fxSpawnTransform.position, fxSpawnTransform.rotation);
        shotEffect.transform.Rotate(180, 0, 0);

        //make sure it plays
        shotEffect.GetComponent<ParticleSystem>().Play(true);

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
                //effect
                SlamEffect(1);
                hitbox.damage = currentAtk * 8;
                hitbox.range = 4;
                hitbox.type = 0;
                break;
            case "combo 1.1":
                hitbox.damage = currentAtk * 12;
                hitbox.range = 6;
                hitbox.type = 1;
                break;
            case "combo 1 follow up":
                //effect
                GunshotEffect();
                //dont parent it to the boss
                hitboxObject.transform.SetParent(null);
                hitbox.damage = currentAtk * 12;
                hitbox.range = 6;
                hitbox.type = 1;
                break;
            case "combo 2":
                hitbox.damage = currentAtk * 8;
                hitbox.range = 4;
                hitbox.type = 0;
                break;
            case "combo 2.1":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 6;
                hitbox.type = 1;
                break;
            case "combo 2 follow up":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 4;
                hitbox.type = 0;
                break;
            case "combo 2 follow up.1":
                //effect
                GunshotEffect();
                //dont parent it to the boss
                hitboxObject.transform.SetParent(null);
                hitbox.damage = currentAtk * 12;
                hitbox.range = 6;
                hitbox.type = 1;
                break;
            case "combo back":
                //effect
                SlamEffect(1);
                hitbox.damage = currentAtk * 8;
                hitbox.range = 4;
                hitbox.type = 1;
                break;
            case "combo overhead":
                //effect
                SlamEffect(1.2f);
                hitbox.damage = currentAtk * 12;
                hitbox.range = 6;
                hitbox.type = 1;
                break;
            case "spin":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 6;
                hitbox.type = 1;
                break;
            case "dash punch":
                hitbox.damage = currentAtk * 10;
                hitbox.range = 4;
                hitbox.type = 1;
                break;
            case "dive punch":
                //effect
                SlamEffect(1.5f);
                hitbox.damage = currentAtk * 12;
                hitbox.range = 8;
                hitbox.type = 1;
                break;
            case "slam":
                //explosion effect
                ExplosionEffect();
                //hitbox stuffs
                hitbox.damage = currentAtk * 15;
                hitbox.range = 10;
                hitbox.type = 1;
                break;
            case "fire bullet":
                //spawn bullets
                GameObject bulletObj = ObjectPool.instance.SpawnObject("bullet", spawnTransform.position, spawnTransform.rotation);
                //slightly spread out the direction, then add force
                bulletObj.transform.Rotate(Random.Range(-7, 7), Random.Range(-7, 7), Random.Range(-7, 7));
                bulletObj.GetComponent<Rigidbody>().AddForce(bulletObj.transform.up * 2000f);
                //muzzle flash effect
                MuzzleEffect();
                //hitbox stuffs
                HitboxBehavior bulletHitbox = bulletObj.GetComponent<HitboxBehavior>();
                bulletHitbox.targetTag = "Player";
                bulletHitbox.damage = currentAtk * 2;
                bulletHitbox.range = 4;
                bulletHitbox.type = 0;
                break;
            case "phase transition":
                hitbox.damage = currentAtk * 8;
                hitbox.range = 6;
                hitbox.type = 1;
                break;
            default:
                break;

        }

    }

}

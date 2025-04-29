using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    private PlayerBehavior player;
    public PlayerAudio playerAudio;
    private StateMachine stateMachine;

    public Transform spawnTransform;
    public Transform fxSpawnTransform;

    public float baseAtk;
    public float currentAtk;

    public float currentEnergy;
    public float maxEnergy;
    public int currentHeat;
    public int maxHeat;

    public bool enhancedSkill;

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
        playerAudio = GetComponent<PlayerAudio>();
        stateMachine = GetComponent<StateMachine>();

        currentAtk = baseAtk;

        currentEnergy = 0;
        currentHeat = 0;
        Actions.UpdatePlayerEnergyBar(this);
        Actions.UpdatePlayerHeatBar(this);

        enhancedSkill = false;
    }

    // Update is called once per frame
    void Update()
    {

    }




    //can call this from somewhere else
    public void GainEnergy(float energyToGain)
    {
        currentEnergy += Mathf.Clamp(energyToGain, 0f, maxEnergy);

        if (currentEnergy <= 0)
            currentEnergy = 0;
        else if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        Actions.UpdatePlayerEnergyBar(this);
    }
    public void LoseEnergy(float energyToLose)
    {
        currentEnergy -= Mathf.Clamp(energyToLose, 0f, maxEnergy);

        if (currentEnergy <= 0)
            currentEnergy = 0;
        else if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        Actions.UpdatePlayerEnergyBar(this);
    }
    public void GainHeat(int heatToGain)
    {
        currentHeat += Mathf.Clamp(heatToGain, 0, maxHeat);

        if (currentEnergy <= 0)
            currentEnergy = 0;
        else if (currentHeat > maxHeat)
            currentHeat = maxHeat;

        Actions.UpdatePlayerHeatBar(this);
    }
    public void LoseHeat(int heatToLose)
    {
        currentHeat -= Mathf.Clamp(heatToLose, 0, maxHeat);

        if (currentEnergy <= 0)
            currentEnergy = 0;
        else if (currentHeat > maxHeat)
            currentHeat = maxHeat;

        Actions.UpdatePlayerHeatBar(this);
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
                //blunt effect
                GameObject bluntObj = ObjectPool.instance.SpawnObject("playerBluntEffect", fxSpawnTransform.position, Random.rotation);
                bluntObj.transform.SetParent(this.transform);
                //make sure it plays
                bluntObj.GetComponent<ParticleSystem>().Play(true);

                //hitbox stuffs
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
                //blunt effect
                GameObject blunt1Obj = ObjectPool.instance.SpawnObject("playerBluntEffect", fxSpawnTransform.position, Random.rotation);
                blunt1Obj.transform.SetParent(this.transform);
                //make sure it plays
                blunt1Obj.GetComponent<ParticleSystem>().Play(true);

                //hitbox stuffs
                hitbox.damage = currentAtk * 8;
                hitbox.armorDamage = 10;
                hitbox.range = 1.5f;
                hitbox.energyOnHit = 5;
                break;
            case "basic 4.1":
                //blunt effect
                GameObject blunt2Obj = ObjectPool.instance.SpawnObject("playerBluntEffect", fxSpawnTransform.position, Random.rotation);
                blunt2Obj.transform.SetParent(this.transform);
                //make sure it plays
                blunt2Obj.GetComponent<ParticleSystem>().Play(true);

                //hitbox stuffs
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
                //blunt effect
                GameObject blunt3Obj = ObjectPool.instance.SpawnObject("playerBluntEffect", fxSpawnTransform.position, Random.rotation);
                blunt3Obj.transform.SetParent(this.transform);
                //make sure it plays
                blunt3Obj.GetComponent<ParticleSystem>().Play(true);

                //hitbox stuffs
                hitbox.damage = currentAtk * 10;
                hitbox.armorDamage = 18;
                hitbox.range = 2;
                hitbox.energyOnHit = 10;
                break;
            case "air 1":
                //blunt effect
                GameObject blunt4Obj = ObjectPool.instance.SpawnObject("playerBluntEffect", fxSpawnTransform.position, Random.rotation);
                blunt4Obj.transform.SetParent(this.transform);
                //make sure it plays
                blunt4Obj.GetComponent<ParticleSystem>().Play(true);

                //hitbox stuffs
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
                //blunt effect
                GameObject blunt5Obj = ObjectPool.instance.SpawnObject("playerBluntEffect", fxSpawnTransform.position, Random.rotation);
                blunt5Obj.transform.SetParent(this.transform);
                //make sure it plays
                blunt5Obj.GetComponent<ParticleSystem>().Play(true);

                //audio
                playerAudio.PlayAudioClip("plungeLand");

                //hitbox stuffs
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
                    multiHitbox.energyOnHit = 0;

                    //audio
                    player.playerAudio.PlayAudioClip("heavyRelease_1");

                    //spend energy and gain heat, doesnt matter if hit
                    LoseEnergy(currentEnergy);
                    GainHeat(1);
                }
                else
                {
                    //explosion effect
                    GameObject explosionObj = ObjectPool.instance.SpawnObject("smallExplosionEffect", spawnTransform.position, spawnTransform.rotation);
                    //make sure it plays
                    explosionObj.GetComponent<ParticleSystem>().Play(true);

                    //hitbox stuffs
                    hitbox.damage = currentAtk * 10;
                    hitbox.armorDamage = 15;
                    hitbox.range = 3;
                    hitbox.energyOnHit = 0;

                    //audio
                    player.playerAudio.PlayAudioClip("heavyRelease");
                }
                break;
            case "skill 1":
                if (enhancedSkill)
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
                if (enhancedSkill)
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
                if (enhancedSkill)
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
                if (enhancedSkill)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHitbox : MonoBehaviour
{
    public float spawnAmount;
    public float spawnInterval;

    public string targetTag;
    public float damage;
    public float armorDamage;
    public float range;
    public float energyOnHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(SpawnHitbox());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnHitbox()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            //spawn and adjust properties
            GameObject hitboxObject = ObjectPool.instance.SpawnObject("hitbox", transform.position, transform.rotation);
            HitboxBehavior hitbox = hitboxObject.GetComponent<HitboxBehavior>();
            hitbox.targetTag = targetTag;
            hitbox.damage = damage;
            hitbox.armorDamage = armorDamage;
            hitbox.range = range;
            hitbox.energyOnHit = energyOnHit;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

}

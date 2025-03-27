using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitboxBehavior : MonoBehaviour
{
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
        //have the value be a variable if u want different durations
        StartCoroutine(DisableAfterTime(0.2f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == targetTag)
        {
            switch (targetTag)
            {
                case "Enemy":
                    otherCollider.GetComponent<EnemyBehavior>().TakeDamage(damage);
                    //otherCollider.GetComponent <EnemyBehavior>().TakeArmorDamage(armorDamage);
                    Actions.GainEnergyOnHit(energyOnHit);
                    break;
                case "Player":
                    otherCollider.GetComponent<PlayerBehavior>().TakeDamage(damage);
                    break;
                default:
                    break;
            }
        }
    }




    private IEnumerator DisableAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        this.gameObject.SetActive(false);
    }

}

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

    //for boss attacks, 0 is light hit, 1 is heavy hit
    public int type;

    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        //have the value be a variable if u want different durations
        StartCoroutine(DisableAfterTime(destroyTime));

        GetComponent<SphereCollider>().radius = range;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SphereCollider>().radius = range;
    }
    
    private IEnumerator DisableAfterTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        this.gameObject.SetActive(false);
    }






    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.tag == targetTag)
        {
            switch (targetTag)
            {
                case "Enemy":
                    otherCollider.GetComponent<EnemyBehavior>().TakeDamage(damage, armorDamage);
                    Actions.GainEnergyOnHit(energyOnHit);
                    //spawn hit effects
                    otherCollider.GetComponent<EnemyBehavior>().HitEffect(otherCollider.ClosestPoint(transform.position));
                    Debug.Log("enemy got hit");
                    break;
                case "Player":
                    otherCollider.GetComponent<PlayerBehavior>().TakeDamage(damage, type);
                    Debug.Log("player got hit");
                    break;
                default:
                    break;
            }

            this.gameObject.SetActive(false);

        }
    }



}

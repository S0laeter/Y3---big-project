using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxBehavior : MonoBehaviour
{
    public string targetTag;

    public float damage;
    public float armorDamage;

    // Start is called before the first frame update
    void Start()
    {
        
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
                case "enemy":
                    otherCollider.GetComponent<EnemyBehavior>().TakeDamage(damage);
                    break;
                case "player":
                    otherCollider.GetComponent<PlayerBehavior>().TakeDamage(damage);
                    break;
                default:
                    break;
            }
        }
    }

}

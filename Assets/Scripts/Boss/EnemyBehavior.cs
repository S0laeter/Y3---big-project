using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float maxHp;
    public float currentHp;

    public float maxArmor;
    public float currentArmor;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        currentArmor = maxArmor;
    }

    // Update is called once per frame
    void Update()
    {
        //hp check
        if (currentHp <= maxHp)
        {
            Die();
        }

        //regen armor
        if (currentArmor <= maxArmor)
        {
            currentArmor += Mathf.Clamp(5f * Time.deltaTime, 0f, maxArmor);
        }

    }

    public void TakeDamage(float damage)
    {
        currentHp -= Mathf.Clamp(damage, 0f, maxHp);
        currentArmor -= Mathf.Clamp(damage * 1.5f, 0f, maxArmor);
    }
    public void Die()
    {

    }

}

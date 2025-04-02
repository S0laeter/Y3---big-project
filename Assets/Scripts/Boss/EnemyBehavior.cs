using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private StateMachine stateMachine;
    public Animator anim;

    public float maxHp;
    public float currentHp;

    public float maxArmor;
    public float currentArmor;

    //atk conditions
    public bool inRange;
    public bool outOfRange;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        anim = GetComponent<Animator>();

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

    public void TakeDamage(float damage, float armorDamage)
    {
        currentHp -= Mathf.Clamp(damage, 0f, maxHp);
        currentArmor -= Mathf.Clamp(armorDamage, 0f, maxArmor);
    }
    public void Die()
    {

    }

}

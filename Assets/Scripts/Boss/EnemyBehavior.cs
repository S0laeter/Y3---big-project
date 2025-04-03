using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private StateMachine stateMachine;
    public CharacterController controller;
    public Animator anim;

    public float maxHp;
    public float currentHp;

    public float maxArmor;
    public float currentArmor;

    public GameObject player;

    //atk conditions
    public bool inRange;
    public bool outOfRange;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");

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



    public void RotateToPlayer(float turnSmoothTime)
    {
        //get direction of player, sometimes its the opposite btw..
        Vector3 relativePosition = this.transform.position - player.transform.position;
        //this is so the character doesnt look up or down, only straight forward
        relativePosition.y = 0f;
        //rotate to player, smoothly
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSmoothTime);
    }
    public void MoveToPosition(Transform target)
    {
        Vector3 offset = transform.position - target.position;
        offset = offset.normalized * 5.0f;
        //normalize it and account for movement speed.
        controller.Move(offset * Time.deltaTime);
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

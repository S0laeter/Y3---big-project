using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private StateMachine playerStateMachine;
    public Transform cam;
    public Rigidbody rb;
    public Animator anim;

    public float maxHp;
    public float currentHp;

    public float maxStamina;
    public float currentStamina;

    public float moveSpeed;
    public float turnSmoothVelocity;

    private PlayerInput playerInput;
    //store these into variables for ease of use
    //moveAction.ReadValue<Vector2>()       ---> use this to continuously read value when the key is down
    //moveAction.triggered                  ---> this is true on the first frame when the key is pressed
    public InputAction moveAction;
    public InputAction dashAction;
    public InputAction jumpAction;

    // Start is called before the first frame update
    void Awake()
    {
        playerStateMachine = GetComponent<StateMachine>();
        cam = GameObject.FindWithTag("MainCamera").transform;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        //assign the input actions
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
        jumpAction = playerInput.actions["Jump"];

        currentHp = maxHp;
        Actions.UpdatePlayerHealthBar(this);
        currentStamina = maxStamina;
        Actions.UpdatePlayerStaminaBar(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHp = Mathf.Clamp(currentHp - damage, 0f, maxHp);
        Actions.UpdatePlayerHealthBar(this);
    }
    public void Heal(float hpToHeal)
    {
        currentHp = Mathf.Clamp(currentHp + hpToHeal, 0f, maxHp);
        Actions.UpdatePlayerHealthBar(this);
    }
    public void ConsumeStamina(float stamina)
    {
        currentStamina = Mathf.Clamp(currentStamina - stamina, 0f, maxStamina);
        Actions.UpdatePlayerStaminaBar(this);
    }


}

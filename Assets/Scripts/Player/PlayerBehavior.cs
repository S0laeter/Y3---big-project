using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    private StateMachine stateMachine;
    public Transform cam;
    public CharacterController controller;
    public Animator anim;

    public float maxHp;
    public float currentHp;

    public float maxStamina;
    public float currentStamina;

    public Vector3 playerVelocity;
    public Vector3 moveDirection;
    public float movementSpeed;
    private float turnSmoothVelocity;

    public float jumpSpeed;
    public float gravity;

    private PlayerInput playerInput;
    //store these into variables for ease of use
    public InputAction moveAction;
    public InputAction dashAction;
    public InputAction jumpAction;

    private GameObject lockedOnEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        cam = GameObject.FindWithTag("MainCamera").transform;
        controller = GetComponent<CharacterController>();
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
        playerVelocity.x = moveDirection.normalized.x * movementSpeed;
        playerVelocity.z = moveDirection.normalized.z * movementSpeed;


        //gravity stuffs
        if (controller.isGrounded)
            playerVelocity.y = -0.5f;
        else
            playerVelocity.y += gravity * Time.deltaTime;


    }





    public void Rotate(float turnSmoothTime)
    {
        if (moveAction.ReadValue<Vector2>() != Vector2.zero)
        {
            //y-axis look rotation relative to camera
            float targetAngle = Mathf.Atan2(moveAction.ReadValue<Vector2>().x, moveAction.ReadValue<Vector2>().y) * Mathf.Rad2Deg + cam.eulerAngles.y;

            //smooth out the angle (change the number at the end, from 0 to 1, the smaller the faster)
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //rotate
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            //set which direction to move
            moveDirection = Quaternion.Euler(0f, smoothAngle, 0f) * Vector3.forward;
        }
    }
    public void SetSpeed(float speed)
    {
        movementSpeed = speed;
    }
    public void Move()
    {
        controller.Move(playerVelocity * Time.deltaTime);
    }
    public void SetVerticalVelocity(float ySpeed)
    {
        playerVelocity.y = ySpeed;
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

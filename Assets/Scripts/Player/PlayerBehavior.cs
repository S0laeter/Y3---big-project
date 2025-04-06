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

    public bool canDash;
    public bool canAirDash;

    public Vector3 playerVelocity;
    public Vector3 moveDirection;
    public float movementSpeed;
    private float turnSmoothVelocity;

    public float gravity;

    private PlayerInput playerInput;
    //store these into variables for ease of use
    public InputAction moveAction;
    public InputAction dashAction;
    public InputAction jumpAction;
    public InputAction normalAction;
    public InputAction heavyAction;
    public InputAction skillAction;

    private GameObject lockedOnEnemy;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        cam = GameObject.FindWithTag("MainCamera").transform;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        //get the input component and assign the input actions
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
        jumpAction = playerInput.actions["Jump"];
        normalAction = playerInput.actions["Normal Attack"];
        heavyAction = playerInput.actions["Heavy Attack"];
        skillAction = playerInput.actions["Skill"];

        //reset stats
        currentHp = maxHp;
        Actions.UpdatePlayerHealthBar(this);
        currentStamina = maxStamina;
        Actions.UpdatePlayerStaminaBar(this);

        canDash = true;
        canAirDash = true;

    }

    // Update is called once per frame
    void Update()
    {
        //hp check
        if (currentHp <= 0)
        {
            Die();
        }

        //regen stamina
        if (currentStamina <= maxStamina)
        {
            currentStamina += Mathf.Clamp(10f * Time.deltaTime, 0f, maxStamina);
        }



        //manual test
        if (Input.GetKeyDown(KeyCode.L))
        {
            stateMachine.SetNextState(new KnockbackState());
        }







        playerVelocity.x = moveDirection.normalized.x * movementSpeed;
        playerVelocity.z = moveDirection.normalized.z * movementSpeed;

        //gravity stuffs
        //might as well put the dash reset here..
        if (controller.isGrounded)
        {
            canAirDash = true;
            playerVelocity.y = -0.5f;
        }
        else
        {
            if (stateMachine.currentState.GetType() == typeof(AirNormal1State)
                || stateMachine.currentState.GetType() == typeof(AirNormal2State)
                || stateMachine.currentState.GetType() == typeof(PlungeState))
                return;
            else
                playerVelocity.y += gravity * Time.deltaTime;
        }

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






    public void TakeDamage(float damage, int type)
    {
        currentHp -= Mathf.Clamp(damage, 0f, maxHp);
        Actions.UpdatePlayerHealthBar(this);

        //if take heavy hit, get knocked back
        if (type == 1)
            stateMachine.SetNextState(new KnockbackState());
    }
    public void ConsumeStamina(float stamina)
    {
        currentStamina -= Mathf.Clamp(stamina, 0f, maxStamina);
        Actions.UpdatePlayerStaminaBar(this);
    }
    public void Die()
    {

    }



    public IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(1f);
        canDash = true;
    }



}

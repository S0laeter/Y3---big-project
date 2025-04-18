using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    private PlayerMechanics playerMechanics;
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
    public Coroutine dashCooldown;

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

    private Transform bossTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerMechanics = GetComponent<PlayerMechanics>();
        stateMachine = GetComponent<StateMachine>();
        
        cam = GameObject.FindWithTag("MainCamera").transform;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        bossTransform = GameObject.FindGameObjectWithTag("Enemy").transform;

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
            stateMachine.SetNextState(new DeathState());
        }

        //regen stamina
        if (currentStamina <= maxStamina)
        {
            currentStamina += Mathf.Clamp(5f * Time.deltaTime, 0f, maxStamina);
            Actions.UpdatePlayerStaminaBar(this);
        }
        



        //manual test
        if (Input.GetKeyDown(KeyCode.L))
        {
            playerMechanics.GainEnergy(100000);
            //stateMachine.SetNextState(new KnockbackState());
        }







        playerVelocity.x = moveDirection.normalized.x * movementSpeed;
        playerVelocity.z = moveDirection.normalized.z * movementSpeed;

        //gravity stuffs
        //might as well put the dash reset here..
        if (controller.isGrounded)
        {
            canAirDash = true;
            playerVelocity.y = -50f;
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

            //smooth out the angle (change the number at the end, from 0 to 1, smaller the faster)
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            //rotate
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            //set which direction to move
            moveDirection = Quaternion.Euler(0f, smoothAngle, 0f) * Vector3.forward;
        }
    }
    public void RotateToBoss(float turnSmoothTime)
    {
        //get direction of player, sometimes its the opposite btw..
        Vector3 relativePosition = bossTransform.position - transform.position;

        //this is so the character doesnt look up or down, only straight forward
        relativePosition.y = 0f;

        //rotate to player, smoothly (bigger the faster)
        Quaternion rotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSmoothTime);

        //set which direction to move
        moveDirection = Quaternion.Slerp(transform.rotation, rotation, turnSmoothTime) * Vector3.forward;
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
        //if already dead, nvm
        if (stateMachine.currentState.GetType() == typeof(GroundForwardDashState))
            return;

        //if dashing, reset dash and take no dmg
        if (stateMachine.currentState.GetType() == typeof(GroundForwardDashState)
            || stateMachine.currentState.GetType() == typeof(GroundBackwardDashState)
            || stateMachine.currentState.GetType() == typeof(AirForwardDashState)
            || stateMachine.currentState.GetType() == typeof(AirBackwardDashState))
        {
            StopCoroutine(dashCooldown);
            canDash = true;
            return;
        }

        //deduct hp, update the hp bar
        currentHp -= Mathf.Clamp(damage, 0f, maxHp);
        Actions.UpdatePlayerHealthBar(this);

        //cant be knocked back during these atks
        if (stateMachine.currentState.GetType() == typeof(Skill1State)
            || stateMachine.currentState.GetType() == typeof(Skill2State)
            || stateMachine.currentState.GetType() == typeof(HeavyChargedState))
            return;

        //if take heavy hit, get knocked back
        if (type == 1)
            stateMachine.SetNextState(new KnockbackState());
    }
    public void ConsumeStamina(float stamina)
    {
        currentStamina -= Mathf.Clamp(stamina, 0f, maxStamina);

        if (currentStamina <= 0f)
            currentStamina = 0f;
        else if (currentStamina > maxStamina)
            currentStamina = maxStamina;

        Actions.UpdatePlayerStaminaBar(this);
    }



    public IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(1f);
        canDash = true;
    }


}

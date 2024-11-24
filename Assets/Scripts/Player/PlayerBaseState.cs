using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : State
{
    protected PlayerController player;

    //duration doesnt need to be the same as animation length, make it slightly shorter to transition early to next attack
    protected float stateDuration;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //getting stuffs
        player = stateMachine.GetComponent<PlayerController>();

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }

    public override void OnExit()
    {
        base.OnExit();

    }

}

public class IdleState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        player.rb.velocity = Vector3.zero;
        Debug.Log("idle");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //when directional buttons are pressed
        if (player.moveAction.triggered)
        {
            stateMachine.SetNextState(new RunState());
        }
        //when jump button is pressed
        else if (player.jumpAction.triggered)
        {
            stateMachine.SetNextState(new JumpState());
        }
        //when dash button is pressed
        else if (player.dashAction.triggered)
        {
            stateMachine.SetNextState(new BackDashState());
        }

    }

}

public class RunState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //run animation
        //anim.SetBool("Running", true);
        Debug.Log("running");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
        {
            //look rotation
            float angle = Mathf.Atan2(player.moveAction.ReadValue<Vector2>().x, player.moveAction.ReadValue<Vector2>().y) * Mathf.Rad2Deg + player.cam.eulerAngles.y;

            //smooth out the angle (only for smooth turning with keyboard stuffs but imma just include it anyway)
            float smoothAngle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, angle, ref player.turnSmoothVelocity, 1f);
            //then rotate to look
            player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //now move
            Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            player.rb.velocity = moveDirection.normalized * player.moveSpeed * Time.deltaTime;
        }
        else
        {
            stateMachine.SetNextState(new IdleState());
        }

    }

}

public class BackDashState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        Debug.Log("back dash");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {

        }

    }

}

public class ForwardDashState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        Debug.Log("forward dash");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {

        }

    }

}

public class SprintState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        Debug.Log("sprinting");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        

    }

}

public class JumpState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        Debug.Log("jump");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        

    }

}
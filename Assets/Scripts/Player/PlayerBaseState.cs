using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : State
{
    protected PlayerBehavior player;

    //duration doesnt need to be the same as animation length, make it slightly shorter to transition early to next attack
    protected float stateDuration;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //getting stuffs
        player = stateMachine.GetComponent<PlayerBehavior>();

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

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

        Debug.Log("idle");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //if they suddenly fall through the floor or smt...
        if (!player.controller.isGrounded)
            stateMachine.SetNextState(new FallState());
        
        if (player.jumpAction.ReadValue<float>() == 1)
        {
            stateMachine.SetNextState(new JumpState());
        }
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            stateMachine.SetNextState(new DashState());
        }
        else if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
        {
            stateMachine.SetNextState(new RunState());
        }

    }

}

public class RunState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        player.SetSpeed(7f);
        Debug.Log("running");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        //if they suddenly fall through the floor or smt...
        if (!player.controller.isGrounded)
            stateMachine.SetNextState(new FallState());

        //when jump button is pressed
        if (player.jumpAction.ReadValue<float>() == 1)
        {
            stateMachine.SetNextState(new JumpState());
        }
        //when dash button is pressed
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            stateMachine.SetNextState(new DashState());
        }
        //when directional buttons are held, move
        else if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
        {
            player.Rotate(0.05f);
            player.Move();
        }
        //if stop moving
        else
        {
            stateMachine.SetNextState(new IdleState());
        }

    }

}

public class JumpState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.05f;

        if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            player.SetSpeed(7f);
        else
            player.SetSpeed(0f);

        player.SetVerticalVelocity(player.jumpSpeed);

        Debug.Log("jump");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.Rotate(0.5f);
        player.Move();

        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextState(new FallState());
        }

    }

}

public class FallState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            player.SetSpeed(7f);
        else
            player.SetSpeed(0f);

        Debug.Log("falling");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.Rotate(0.5f);
        player.Move();

        if (player.controller.isGrounded)
        {
            stateMachine.SetNextState(new IdleState());
        }

    }

}

public class DashState : PlayerBaseState
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


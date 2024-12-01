using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : State
{
    protected PlayerBehavior player;

    //duration doesnt need to be the same as animation length, make it slightly shorter to transition early to next attack
    protected float stateDuration;

    //use this for queueing next attack / input buffering
    protected bool normalTrigger;
    protected bool heavyTrigger;
    protected bool skillTrigger;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //getting stuffs
        player = stateMachine.GetComponent<PlayerBehavior>();

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.normalAction.triggered)
        {
            normalTrigger = true;
        }
        else if (player.heavyAction.triggered)
        {
            heavyTrigger = true;
        }
        else if (player.skillAction.triggered)
        {
            skillTrigger = true;
        }

    }

    public override void OnExit()
    {
        base.OnExit();

    }

}

public class PlayerIdleState : PlayerBaseState
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

        if (normalTrigger)
            stateMachine.SetNextState(new Normal1State());

    }

}

public class DeadState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);


        Debug.Log("player dead");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();



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
            stateMachine.SetNextState(new PlayerIdleState());
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
            stateMachine.SetNextState(new PlayerIdleState());
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

public class Normal1State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;
        Debug.Log("normal atk 1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal2State());
            }
            else
                stateMachine.SetNextState(new PlayerIdleState());
        }

    }

}
public class Normal2State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;
        Debug.Log("normal atk 2");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal3State());
            }
            else
                stateMachine.SetNextState(new PlayerIdleState());
        }

    }

}
public class Normal3State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;
        Debug.Log("normal atk 3");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal4State());
            }
            else
                stateMachine.SetNextState(new PlayerIdleState());
        }

    }

}
public class Normal4State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;
        Debug.Log("normal atk 4");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (heavyTrigger)
            {

            }
            else
                stateMachine.SetNextState(new PlayerIdleState());
        }

    }

}
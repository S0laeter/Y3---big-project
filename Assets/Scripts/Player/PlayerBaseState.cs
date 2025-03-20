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

        normalTrigger = false;
        heavyTrigger = false;
        skillTrigger = false;

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

        player.anim.SetTrigger("idle");
        Debug.Log("idle");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //input mid state
        if (skillTrigger)
            stateMachine.SetNextState(new SkillChargingState());
        else if (normalTrigger)
            stateMachine.SetNextState(new Normal1State());
        else if (heavyTrigger)
            stateMachine.SetNextState(new HeavyChargingState());

        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            {
                player.Rotate(0f);
                stateMachine.SetNextState(new GroundForwardDashState());
            }
            else
                stateMachine.SetNextState(new GroundBackwardDashState());
        }
        else if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            stateMachine.SetNextState(new RunState());
        

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

        player.anim.SetTrigger("moveRun");
        Debug.Log("running");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //input mid state
        if (skillTrigger)
            stateMachine.SetNextState(new SkillChargingState());
        else if (normalTrigger)
            stateMachine.SetNextState(new Normal1State());
        else if (heavyTrigger)
            stateMachine.SetNextState(new HeavyChargingState());

        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new GroundForwardDashState());
        else if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
        {
            player.Rotate(0.05f);
            player.Move();
        }
        else
            stateMachine.SetNextStateToMain();

    }

}
public class SprintState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        player.SetSpeed(10f);

        player.anim.SetTrigger("moveSprint");
        Debug.Log("sprinting");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //input mid state
        if (skillTrigger)
            stateMachine.SetNextState(new SkillChargingState());
        else if (normalTrigger)
            stateMachine.SetNextState(new Normal1State());
        else if (heavyTrigger)
            stateMachine.SetNextState(new HeavyChargingState());

        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new GroundForwardDashState());
        else if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
        {
            player.Rotate(0.05f);
            player.Move();
        }
        else
            stateMachine.SetNextStateToMain();

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

        player.anim.SetTrigger("moveJump");
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

        player.anim.SetTrigger("moveFall");
        Debug.Log("falling");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.Rotate(0.5f);
        player.Move();

        if (player.controller.isGrounded)
        {
            stateMachine.SetNextStateToMain();
        }

    }

}

public class GroundForwardDashState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1f;

        player.anim.SetTrigger("dashGroundForward");
        Debug.Log("ground forward dash");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (skillTrigger)
                stateMachine.SetNextState(new SkillChargingState());
            else if (normalTrigger)
                stateMachine.SetNextState(new Normal1State());
            else if (heavyTrigger)
                stateMachine.SetNextState(new HeavyChargingState());

            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                stateMachine.SetNextState(new SprintState());
            else
                stateMachine.SetNextState(new PlayerIdleState());
        }

    }

}
public class GroundBackwardDashState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1f;

        player.anim.SetTrigger("dashGroundForward");
        Debug.Log("ground forward dash");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (skillTrigger)
                stateMachine.SetNextState(new SkillChargingState());
            else if (normalTrigger)
                stateMachine.SetNextState(new Normal1State());
            else if (heavyTrigger)
                stateMachine.SetNextState(new HeavyChargingState());

            else
                stateMachine.SetNextState(new PlayerIdleState());
        }

    }

}

public class Normal1State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;

        player.anim.SetTrigger("atkBasic1");
        Debug.Log("normal atk 1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //jump and dash cancel
        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            {
                player.Rotate(0f);
                stateMachine.SetNextState(new GroundForwardDashState());
            }
            else
                stateMachine.SetNextState(new GroundBackwardDashState());
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal2State());
            }
            else
                stateMachine.SetNextStateToMain();
        }

    }

}
public class Normal2State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;

        player.anim.SetTrigger("atkBasic2");
        Debug.Log("normal atk 2");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //jump and dash cancel
        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            {
                player.Rotate(0f);
                stateMachine.SetNextState(new GroundForwardDashState());
            }
            else
                stateMachine.SetNextState(new GroundBackwardDashState());
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal3State());
            }
            else
                stateMachine.SetNextStateToMain();
        }

    }

}
public class Normal3State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;

        player.anim.SetTrigger("atkBasic3");
        Debug.Log("normal atk 3");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //jump and dash cancel
        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            {
                player.Rotate(0f);
                stateMachine.SetNextState(new GroundForwardDashState());
            }
            else
                stateMachine.SetNextState(new GroundBackwardDashState());
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal4State());
            }
            else
                stateMachine.SetNextStateToMain();
        }

    }

}
public class Normal4State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;

        player.anim.SetTrigger("atkBasic4");
        Debug.Log("normal atk 4");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //jump and dash cancel
        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            {
                player.Rotate(0f);
                stateMachine.SetNextState(new GroundForwardDashState());
            }
            else
                stateMachine.SetNextState(new GroundBackwardDashState());
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal5State());
            }
            else
                stateMachine.SetNextStateToMain();
        }

    }

}
public class Normal5State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.5f;

        player.anim.SetTrigger("atkBasic5");
        Debug.Log("normal atk 5");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //jump and dash cancel
        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            {
                player.Rotate(0f);
                stateMachine.SetNextState(new GroundForwardDashState());
            }
            else
                stateMachine.SetNextState(new GroundBackwardDashState());
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (heavyTrigger)
            {

            }
            else
                stateMachine.SetNextStateToMain();
        }

    }

}

public class HeavyChargingState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1f;

        player.anim.SetTrigger("atkHeavyCharging");
        Debug.Log("heavy charging");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //jump and dash cancel
        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            {
                player.Rotate(0f);
                stateMachine.SetNextState(new GroundForwardDashState());
            }
            else
                stateMachine.SetNextState(new GroundBackwardDashState());
        }

        if (player.heavyAction.ReadValue<float>() == 1)
        {
            //increase energy over time
        }
        else
        {
            //after state duration
            if (fixedTime >= stateDuration)
            {
                stateMachine.SetNextState(new HeavyChargedState());
            }
        }

        

    }

}
public class HeavyChargedState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2f;

        player.anim.SetTrigger("atkHeavyCharged");
        Debug.Log("heavy released");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }

}

public class SkillChargingState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.5f;

        player.anim.SetTrigger("atkSkillCharging");
        Debug.Log("skill charging");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //jump and dash cancel
        if (player.jumpAction.ReadValue<float>() == 1)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.ReadValue<float>() == 1)
        {
            if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            {
                player.Rotate(0f);
                stateMachine.SetNextState(new GroundForwardDashState());
            }
            else
                stateMachine.SetNextState(new GroundBackwardDashState());
        }

        if (player.heavyAction.ReadValue<float>() == 1)
        {
            //after holding for a while
            if (fixedTime >= 1.5f)
            {
                //ult here.......
            }
        }
        else
        {
            //after state duration
            if (fixedTime >= stateDuration)
            {
                stateMachine.SetNextState(new Skill1State());
            }
            
        }



    }

}
public class Skill1State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2f;

        player.anim.SetTrigger("atkSkill1");
        Debug.Log("skill 1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (skillTrigger)
            {
                stateMachine.SetNextState(new Skill2State());
            }
            else
                stateMachine.SetNextStateToMain();
        }

    }

}
public class Skill2State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2f;

        player.anim.SetTrigger("atkSkill2");
        Debug.Log("skill 2");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (skillTrigger)
            {
                stateMachine.SetNextState(new Skill1State());
            }
            else
                stateMachine.SetNextStateToMain();
        }

    }

}

//do airborne atks later
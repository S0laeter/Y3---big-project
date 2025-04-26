using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : State
{
    protected PlayerBehavior player;
    protected PlayerMechanics playerMechanics;

    //duration doesnt need to be the same as animation length, make it slightly shorter to transition early to next attack
    protected float stateDuration;

    //use this for queueing next attack / input buffering
    //if queueing, use this
    //if on click but not queueing, use .triggered
    //if holding button, use .ReadValue<float>() == 1
    protected bool normalTrigger;
    protected bool heavyTrigger;
    protected bool skillTrigger;
    protected bool jumpTrigger;
    protected bool dashTrigger;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //getting stuffs
        player = stateMachine.GetComponent<PlayerBehavior>();
        playerMechanics = stateMachine.GetComponent<PlayerMechanics>();

        dashTrigger = false;
        jumpTrigger = false;
        normalTrigger = false;
        heavyTrigger = false;
        skillTrigger = false;

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.dashAction.triggered)
        {
            dashTrigger = true;
        }
        else if (player.jumpAction.triggered)
        {
            jumpTrigger = true;
        }
        else if (player.normalAction.triggered)
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

        player.SetSpeed(0f);

        player.anim.ResetTrigger("moveFall");
        player.anim.SetTrigger("idle");
        Debug.Log("idle");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //input mid state
        if (skillTrigger)
            stateMachine.SetNextState(new SkillChargingState());
        else if (normalTrigger)
            stateMachine.SetNextState(new Normal1State());
        else if (heavyTrigger)
            stateMachine.SetNextState(new HeavyChargingState());

        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }
        else if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
            stateMachine.SetNextState(new RunState());
        

    }

}

public class DeathState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        player.anim.SetTrigger("dead");
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

        player.anim.ResetTrigger("idle");
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

        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                stateMachine.SetNextState(new GroundForwardDashState());
            }
        }
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

        player.anim.ResetTrigger("idle");
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

        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                stateMachine.SetNextState(new GroundForwardDashState());
            }
        }
        else if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
        {
            player.Rotate(0.2f);
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

        //jump force
        player.SetVerticalVelocity(15f);

        player.anim.ResetTrigger("idle");
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

        //jump and dash cancel
        if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canAirDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new AirForwardDashState());
                }
                else
                {
                    stateMachine.SetNextState(new AirBackwardDashState());
                }
            }
        }

        if (player.normalAction.triggered)
            stateMachine.SetNextState(new AirNormal1State());
        else if (player.heavyAction.triggered)
            stateMachine.SetNextState(new PlungeState());
        else if (player.skillAction.triggered)
            stateMachine.SetNextState(new PlungeState());


    }

}

public class KnockbackState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.35f;

        player.RotateToBoss(1f);

        player.SetSpeed(-9f);
        //get launched up
        player.SetVerticalVelocity(12f);

        player.anim.ResetTrigger("moveFall");
        player.anim.SetTrigger("knockback");
        Debug.Log("knockback");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.Move();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (player.controller.isGrounded)
                stateMachine.SetNextState(new KnockbackRecoveryState());
        }
            

    }

}
public class KnockbackRecoveryState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.5f;

        player.anim.SetTrigger("knockbackRecovery");
        Debug.Log("knockback recovery");
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

public class GroundForwardDashState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.40f;

        player.ConsumeStamina(10f);
        player.dashCooldown = player.StartCoroutine(player.DashCooldown());

        player.Rotate(0.001f);

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

            if (jumpTrigger)
                stateMachine.SetNextState(new JumpState());
            else if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
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

        stateDuration = 0.40f;

        player.ConsumeStamina(10f);
        player.dashCooldown = player.StartCoroutine(player.DashCooldown());

        player.anim.SetTrigger("dashGroundBackward");
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

            if (jumpTrigger)
                stateMachine.SetNextState(new JumpState());
            else
                stateMachine.SetNextState(new PlayerIdleState());
        }

    }

}
public class AirForwardDashState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.40f;

        player.ConsumeStamina(10f);
        player.canAirDash = false;

        //flip up
        player.SetVerticalVelocity(10f);
        player.SetSpeed(7f);
        player.Rotate(0.001f);

        player.anim.SetTrigger("dashAirForward");
        Debug.Log("air forward dash");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.Move();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
                stateMachine.SetNextState(new AirNormal1State());
            else if (heavyTrigger)
                stateMachine.SetNextState(new PlungeState());
            else if (skillTrigger)
                stateMachine.SetNextState(new PlungeState());
            else
                stateMachine.SetNextState(new FallState());

        }

    }

}
public class AirBackwardDashState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.40f;

        player.ConsumeStamina(10f);
        player.canAirDash = false;

        //flip up
        player.SetVerticalVelocity(10f);
        player.SetSpeed(-7f);

        player.anim.SetTrigger("dashAirBackward");
        Debug.Log("air forward dash");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.Move();

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
                stateMachine.SetNextState(new AirNormal1State());
            else if (heavyTrigger)
                stateMachine.SetNextState(new PlungeState());
            else if (skillTrigger)
                stateMachine.SetNextState(new PlungeState());
            else
                stateMachine.SetNextState(new FallState());

        }

    }

}

public class Normal1State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.40f;

        player.SetSpeed(0f);

        player.anim.SetTrigger("atkBasic1");
        Debug.Log("normal atk 1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }

        //after state duration
        if (fixedTime >= stateDuration + 0.2f)
            stateMachine.SetNextStateToMain();
        else if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
                stateMachine.SetNextState(new Normal2State());
            else if (skillTrigger)
                stateMachine.SetNextState(new SkillChargingState());
            else if (heavyTrigger)
                stateMachine.SetNextState(new HeavyChargingState());
        }

    }

}
public class Normal2State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.35f;

        player.SetSpeed(0f);

        player.anim.SetTrigger("atkBasic2");
        Debug.Log("normal atk 2");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }

        //after state duration
        if (fixedTime >= stateDuration + 0.2f)
            stateMachine.SetNextStateToMain();
        else if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
                stateMachine.SetNextState(new Normal3State());
            else if (skillTrigger)
                stateMachine.SetNextState(new SkillChargingState());
            else if (heavyTrigger)
                stateMachine.SetNextState(new HeavyChargingState());
        }

    }

}
public class Normal3State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.40f;

        player.SetSpeed(0f);

        player.anim.SetTrigger("atkBasic3");
        Debug.Log("normal atk 3");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }

        //after state duration
        if (fixedTime >= stateDuration + 0.3f)
            stateMachine.SetNextStateToMain();
        else if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
                stateMachine.SetNextState(new Normal4State());
            else if (skillTrigger)
                stateMachine.SetNextState(new SkillChargingState());
            else if (heavyTrigger)
                stateMachine.SetNextState(new HeavyChargingState());
        }

    }

}
public class Normal4State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.80f;

        player.SetSpeed(0f);

        player.anim.SetTrigger("atkBasic4");
        Debug.Log("normal atk 4");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }

        //after state duration
        if (fixedTime >= stateDuration + 0.2f)
            stateMachine.SetNextStateToMain();
        else if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
                stateMachine.SetNextState(new Normal5State());
            else if (skillTrigger)
                stateMachine.SetNextState(new SkillChargingState());
            else if (heavyTrigger)
                stateMachine.SetNextState(new HeavyChargingState());
        }

    }

}
public class Normal5State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.0f;

        player.SetSpeed(0f);

        player.anim.SetTrigger("atkBasic5");
        Debug.Log("normal atk 5");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }

        //after state duration
        if (fixedTime >= stateDuration + 0.2f)
            stateMachine.SetNextStateToMain();
        else if (fixedTime >= stateDuration)
        {
            if (skillTrigger)
                stateMachine.SetNextState(new SkillChargingState());
            else if (heavyTrigger)
                stateMachine.SetNextState(new HeavyChargingState());
        }

    }

}

public class HeavyChargingState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.20f;

        player.SetSpeed(0f);

        player.anim.SetTrigger("atkHeavyCharging");
        Debug.Log("heavy charging");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }

        //holding
        if (player.heavyAction.ReadValue<float>() == 1)
        {
            playerMechanics.GainEnergy(15f * Time.deltaTime);
        }
        //release
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

        stateDuration = 0.60f;

        player.SetSpeed(0f);

        player.anim.SetTrigger("atkHeavyCharged");
        Debug.Log("heavy released");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //after state duration
        if (fixedTime >= stateDuration + 0.2f)
            stateMachine.SetNextStateToMain();
        else if (fixedTime >= stateDuration)
        {
            if (player.jumpAction.triggered)
                stateMachine.SetNextState(new JumpState());
            else if (player.dashAction.triggered)
            {
                if (player.currentStamina > 10 && player.canDash)
                {
                    if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                    {
                        stateMachine.SetNextState(new GroundForwardDashState());
                    }
                    else
                        stateMachine.SetNextState(new GroundBackwardDashState());
                }
            }
            //follow up skill 1
            if (skillTrigger)
            {
                stateMachine.SetNextState(new SkillChargingState());
            }
            //follow up basic atk
            else if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal4State());
            }

        }

    }

}

public class SkillChargingState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.15f;

        player.SetSpeed(0f);

        player.animSword.SetTrigger("transformed");
        player.anim.SetTrigger("atkSkillCharging");
        Debug.Log("skill charging");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            //holding, and has full heat
            if (player.skillAction.ReadValue<float>() == 1 && playerMechanics.currentHeat >= playerMechanics.maxHeat)
            {
                //after holding for a while
                if (fixedTime >= 1.5f)
                {
                    //ult here.......
                }
            }
            //release
            else
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

        stateDuration = 0.85f;

        //if got heat, enhance skill and spend heat
        if (playerMechanics.currentHeat > 0)
        {
            playerMechanics.enhancedSkill = true;
            playerMechanics.LoseHeat(1);
        }
        else
        {
            playerMechanics.enhancedSkill= false;
        }

        player.SetSpeed(0f);

        player.animSword.SetTrigger("transformedInstant");
        player.anim.SetTrigger("atkSkill1");
        Debug.Log("skill 1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //after state duration
        if (fixedTime >= stateDuration + 0.1f)
        {
            if (player.jumpAction.triggered)
            {
                player.animSword.SetTrigger("instantNormal");
                stateMachine.SetNextState(new JumpState());
            }
            else if (player.dashAction.triggered)
            {
                if (player.currentStamina > 10 && player.canDash)
                {
                    player.animSword.SetTrigger("instantNormal");

                    if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                    {
                        stateMachine.SetNextState(new GroundForwardDashState());
                    }
                    else
                        stateMachine.SetNextState(new GroundBackwardDashState());
                }
            }
            //follow up basic atk
            else if (normalTrigger)
            {
                player.animSword.SetTrigger("instantNormal");
                stateMachine.SetNextState(new Normal1State());
            }
            else
            {
                player.animSword.SetTrigger("normal");
                stateMachine.SetNextStateToMain();
            }

        }
            
        else if (fixedTime >= stateDuration)
        {
            //follow up skill 2
            if (skillTrigger)
            {
                stateMachine.SetNextState(new Skill2State());
            }

        }

    }

}
public class Skill2State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.30f;

        //if got heat, enhance skill and spend heat
        if (playerMechanics.currentHeat > 0)
        {
            playerMechanics.enhancedSkill = true;
            playerMechanics.LoseHeat(1);
        }
        else
        {
            playerMechanics.enhancedSkill = false;
        }

        player.SetSpeed(0f);

        player.animSword.SetTrigger("transformedInstant");
        player.anim.SetTrigger("atkSkill2");
        Debug.Log("skill 2");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //after state duration
        if (fixedTime >= stateDuration + 0.2f)
        {
            player.animSword.SetTrigger("normal");
            stateMachine.SetNextStateToMain();
        }
        else if (fixedTime >= stateDuration)
        {

            if (player.jumpAction.triggered)
            {
                player.animSword.SetTrigger("instantNormal");
                stateMachine.SetNextState(new JumpState());
            }
            else if (player.dashAction.triggered)
            {
                if (player.currentStamina > 10 && player.canDash)
                {
                    player.animSword.SetTrigger("instantNormal");

                    if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                    {
                        stateMachine.SetNextState(new GroundForwardDashState());
                    }
                    else
                        stateMachine.SetNextState(new GroundBackwardDashState());
                }
            }
            //follow up basic atk
            if (normalTrigger)
            {
                player.animSword.SetTrigger("instantNormal");

                stateMachine.SetNextState(new Normal3State());
            }


        }

    }

}

public class AirNormal1State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.50f;

        player.SetSpeed(0f);
        player.SetVerticalVelocity(0f);

        player.anim.SetTrigger("atkAir1");
        Debug.Log("airborne atk 1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canAirDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new AirForwardDashState());
                }
                else
                {
                    stateMachine.SetNextState(new AirBackwardDashState());
                }
            }
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
                stateMachine.SetNextState(new AirNormal2State());
            else if (skillTrigger)
                stateMachine.SetNextState(new PlungeState());
            else if (heavyTrigger)
                stateMachine.SetNextState(new PlungeState());
            else
                stateMachine.SetNextState(new FallState());
        }

    }

}
public class AirNormal2State : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.60f;

        player.SetSpeed(0f);
        player.SetVerticalVelocity(0f);

        player.anim.SetTrigger("atkAir2");
        Debug.Log("airborne atk 2");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.2f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canAirDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new AirForwardDashState());
                }
                else
                {
                    stateMachine.SetNextState(new AirBackwardDashState());
                }
            }
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            if (normalTrigger)
                stateMachine.SetNextState(new PlungeState());
            else if (skillTrigger)
                stateMachine.SetNextState(new PlungeState());
            else if (heavyTrigger)
                stateMachine.SetNextState(new PlungeState());
            else
                stateMachine.SetNextState(new FallState());
        }

    }

}
public class PlungeState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        
        //float up a bit
        player.SetVerticalVelocity(3f);
        player.SetSpeed(7f);

        player.anim.ResetTrigger("moveFall");
        player.anim.SetTrigger("atkAirPlunge");
        Debug.Log("plunge atk");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.4f)
        {
            player.RotateToBoss(0.5f);
        }

        //jump and dash cancel
        if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canAirDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new AirForwardDashState());
                }
                else
                {
                    stateMachine.SetNextState(new AirBackwardDashState());
                }
            }
        }

        //plunge down after a bit
        if (fixedTime > 0.5f)
        {
            player.SetVerticalVelocity(-40f);
            player.SetSpeed(12f);
        }

        player.Move();

        if (player.controller.isGrounded)
        {
            //deal damage right when touching the ground
            playerMechanics.SpawnHitbox("plunge");

            stateMachine.SetNextState(new PlungeLandState());
        }

    }

}
public class PlungeLandState : PlayerBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.5f;

        player.SetSpeed(0f);

        player.anim.ResetTrigger("moveFall");
        player.anim.SetTrigger("idle");
        Debug.Log("plunge landing");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //or rather stop moving, the speed is 0
        player.Move();

        //jump and dash cancel
        if (player.jumpAction.triggered)
            stateMachine.SetNextState(new JumpState());
        else if (player.dashAction.triggered)
        {
            if (player.currentStamina > 10 && player.canDash)
            {
                if (player.moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    stateMachine.SetNextState(new GroundForwardDashState());
                }
                else
                    stateMachine.SetNextState(new GroundBackwardDashState());
            }
        }

        if (fixedTime >= stateDuration)
        {
            //follow up skill 1
            if (skillTrigger)
            {
                stateMachine.SetNextState(new SkillChargingState());
            }
            //follow up basic atk
            else if (normalTrigger)
            {
                stateMachine.SetNextState(new Normal4State());
            }
            else
                stateMachine.SetNextStateToMain();
        }
            

    }

}
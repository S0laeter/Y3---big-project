using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : State
{
    protected EnemyBehavior enemy;

    //duration doesnt need to be the same as animation length, make it slightly shorter to transition early to next attack
    protected float stateDuration;

    //just for the rapid fire
    protected float shootTime;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //getting stuffs
        enemy = stateMachine.GetComponent<EnemyBehavior>();

        Physics.IgnoreLayerCollision(10, 15, false);

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

public class EnemyIdleState : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        enemy.anim.SetTrigger("idle");
        Debug.Log("idle");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //if still in phase 1 and not transitioned yet
        if (enemy.currentPhase == 1 && enemy.currentHp <= enemy.maxHp * 0.5f)
        {
            stateMachine.SetNextState(new EnemyPhaseTransition());
            return;
        }

        //moveset
        switch (enemy.currentPhase)
        {
            
            case 1:
                //when far away
                if (enemy.outOfRange)
                {
                    float rand = Random.Range(0f, 100f);
                    if (rand <= 20)
                        stateMachine.SetNextState(new EnemyDashPunch());
                    else
                        stateMachine.SetNextState(new EnemyDivePunch());
                }
                //when close
                else if (enemy.inRange)
                {
                    float rand = Random.Range(0f, 100f);
                    if (rand <= 18)
                        stateMachine.SetNextState(new EnemyCombo1());
                    else if (rand <= 36)
                        stateMachine.SetNextState(new EnemyCombo2());
                    else if (rand <= 54)
                        stateMachine.SetNextState(new EnemyComboOverhead());
                    else if (rand <= 70)
                        stateMachine.SetNextState(new EnemySpin());
                    else
                        stateMachine.SetNextState(new EnemyDash());
                }
                break;
            
            case 2:
                //when far away
                if (enemy.outOfRange)
                {
                    float rand = Random.Range(0f, 100f);
                    if (rand <= 10)
                        stateMachine.SetNextState(new EnemyDashPunch());
                    else if (rand <= 30)
                        stateMachine.SetNextState(new EnemyDivePunch());
                    else if (rand <= 50)
                        stateMachine.SetNextState(new EnemySlam());
                    else
                        stateMachine.SetNextState(new EnemyRapidFire());
                }
                //when close
                else if (enemy.inRange)
                {
                    float rand = Random.Range(0f, 100f);
                    if (rand <= 15)
                        stateMachine.SetNextState(new EnemyCombo1());
                    else if (rand <= 30)
                        stateMachine.SetNextState(new EnemyCombo2());
                    else if (rand <= 45)
                        stateMachine.SetNextState(new EnemyComboOverhead());
                    else if (rand <= 60)
                        stateMachine.SetNextState(new EnemyComboBack());
                    else if (rand <= 75)
                        stateMachine.SetNextState(new EnemySpin());
                    else
                        stateMachine.SetNextState(new EnemyDash());
                }
                break;

            //for testing
            default:
                break;
        }



    }

}
public class EnemyStaggeredState : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 5f;

        enemy.anim.ResetTrigger("idle");
        enemy.anim.SetTrigger("staggered");
        Debug.Log("enemy staggered");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedTime >= stateDuration)
        {
            //regen all armor when wake up
            enemy.currentArmor = enemy.maxArmor;
            stateMachine.SetNextState(new EnemyComboBack());
        }

    }

}
public class EnemyPhaseTransition : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 3.5f;

        enemy.currentPhase = 2;

        enemy.anim.SetTrigger("phase transition");
        Debug.Log("enemy going into phase 2");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.2f)
        {
            enemy.RotateToPlayer(0.4f);
        }

        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyDeathState : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        enemy.anim.SetTrigger("dead");
        Debug.Log("enemy dead");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();


    }

}

public class EnemyDash : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.1f;

        float rand = Random.Range(0f, 100f);
        if (rand <= 33f)
        {
            enemy.anim.SetTrigger("dash back");
        }
        else if (rand <= 66f)
        {
            enemy.anim.SetTrigger("dash right");
        }
        else
        {
            enemy.anim.SetTrigger("dash left");
        }

        Debug.Log("enemy dashing");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.1f)
        {
            enemy.RotateToPlayer(0.4f);
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyCombo1 : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.5f;

        enemy.anim.SetTrigger("combo 1");

        Debug.Log("enemy combo 1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.2f)
        {
            enemy.RotateToPlayer(0.2f);
        }
        if (fixedTime > 0.6f && fixedTime <= 0.8f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //after state duration
        if (fixedTime >= 1.5f)
        {
            if (enemy.currentPhase == 2)
            {
                if (enemy.inRange)
                    stateMachine.SetNextState(new EnemyCombo1Followup());
            }
        }
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyCombo1Followup : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.2f;

        enemy.anim.SetTrigger("combo 1 followup");

        Debug.Log("enemy combo 1.1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.3f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyCombo2 : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.3f;

        enemy.anim.SetTrigger("combo 2");

        Debug.Log("enemy combo 2");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.3f)
        {
            enemy.RotateToPlayer(0.2f);
        }
        if (fixedTime > 0.7f && fixedTime <= 0.9f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //after state duration
        if (fixedTime >= 1.4f)
        {
            if (enemy.currentPhase == 2)
            {
                if (enemy.inRange)
                    stateMachine.SetNextState(new EnemyCombo2Followup());
            }
        }
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyCombo2Followup : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.9f;

        enemy.anim.SetTrigger("combo 2 followup");

        Debug.Log("enemy combo 2.1");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.2f)
        {
            enemy.RotateToPlayer(0.2f);
        }
        if (fixedTime > 0.7f && fixedTime <= 0.10f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyComboBack : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 1.8f;

        enemy.anim.SetTrigger("combo back");

        Debug.Log("enemy combo back");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.4f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //after state duration
        if (fixedTime >= 0.9f)
        {
            if (enemy.currentPhase == 2)
            {
                if (enemy.inRange)
                    stateMachine.SetNextState(new EnemyCombo2Followup());
            }
        }
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyComboOverhead : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.0f;

        enemy.anim.SetTrigger("combo overhead");

        Debug.Log("enemy combo overhead");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.6f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyRapidFire : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 3.6f;

        enemy.anim.SetTrigger("rapid fire");

        Debug.Log("enemy rapid fire");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //shoot
        if (fixedTime > 0.7f && fixedTime <= 2.5f)
        {
            shootTime += Time.deltaTime;
            while (shootTime >= 0.1f)
            {
                enemy.SpawnHitbox("fire bullet");
                shootTime -= 0.1f;
            }
        }

        //rotate
        if (fixedTime <= 3.0f)
        {
            enemy.RotateToPlayer(0.1f);
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemySpin : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.5f;

        enemy.anim.SetTrigger("spin");

        Debug.Log("enemy spin");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.5f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //ignore collision
        if (fixedTime > 0.7f && fixedTime <= 1.1f)
            Physics.IgnoreLayerCollision(10, 15, true);
        else
            Physics.IgnoreLayerCollision(10, 15, false);

        //after state duration
        if (fixedTime >= 1.7f)
        {
            if (enemy.currentPhase == 2)
            {
                if (enemy.inRange)
                    stateMachine.SetNextState(new EnemyComboOverhead());
            }
        }
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyDashPunch : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.5f;

        enemy.anim.SetTrigger("dash punch");

        Debug.Log("enemy dash punch");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.8f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //ignore collision
        if (fixedTime > 0.7f && fixedTime <= 1.1f)
            Physics.IgnoreLayerCollision(10, 15, true);
        else
            Physics.IgnoreLayerCollision(10, 15, false);

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemyDivePunch : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.5f;

        enemy.anim.SetTrigger("dive punch");

        Debug.Log("enemy dive punch");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime > 0.2f && fixedTime <= 0.8f)
        {
            enemy.RotateToPlayer(0.2f);
        }

        //move to player
        if (fixedTime > 0.7f && fixedTime <= 1.0f && enemy.outOfRange)
        {
            enemy.agent.SetDestination(enemy.playerTransform.position);
        }
        else
        {
            enemy.agent.SetDestination(enemy.transform.position);
            enemy.agent.Warp(enemy.transform.position);
        }

        //after state duration
        if (fixedTime >= 1.7f)
        {
            if (enemy.currentPhase == 2)
            {
                if (enemy.inRange)
                    stateMachine.SetNextState(new EnemyCombo2Followup());
            }
        }
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
public class EnemySlam : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.9f;

        enemy.anim.SetTrigger("slam");

        Debug.Log("enemy slam");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime > 0.6f && fixedTime <= 1.25f)
        {
            enemy.RotateToPlayer(0.2f);
        }
        //move to player
        if (fixedTime > 0.6f && fixedTime <= 1.25f && enemy.outOfRange)
        {
            enemy.agent.SetDestination(enemy.playerTransform.position);
        }
        else
        {
            enemy.agent.SetDestination(enemy.transform.position);
            enemy.agent.Warp(enemy.transform.position);
        }

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
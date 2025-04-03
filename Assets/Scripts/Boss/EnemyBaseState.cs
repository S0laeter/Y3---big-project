using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : State
{
    protected EnemyBehavior enemy;

    //duration doesnt need to be the same as animation length, make it slightly shorter to transition early to next attack
    protected float stateDuration;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        //getting stuffs
        enemy = stateMachine.GetComponent<EnemyBehavior>();

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

        enemy.RotateToPlayer(0.4f);

        /*if (enemy.inRange)
        {
            //melee atk
        }
        else if (enemy.outOfRange)
        {
            //ranged or rush atk
        }*/
            


    }

}

public class EnemyDash : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 0.5f;

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
        if (fixedTime <= 0.2f)
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
public class EnemyCombo1 : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.2f;

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

        stateDuration = 1.9f;

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

        stateDuration = 2.0f;

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

        stateDuration = 2.6f;

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

        stateDuration = 1.5f;

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

        stateDuration = 1.7f;

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

        stateDuration = 3.3f;

        enemy.anim.SetTrigger("rapid fire");

        Debug.Log("enemy rapid fire");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 3.0f)
        {
            enemy.RotateToPlayer(0.6f);
        }

        //after state duration
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

        stateDuration = 2.2f;

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

        stateDuration = 2.2f;

        enemy.anim.SetTrigger("dive punch");

        Debug.Log("enemy dive punch");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        //rotate
        if (fixedTime <= 0.8f)
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
public class EnemySpin : EnemyBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);

        stateDuration = 2.2f;

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

        //after state duration
        if (fixedTime >= stateDuration)
        {
            stateMachine.SetNextStateToMain();
        }

    }
}
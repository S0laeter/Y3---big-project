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
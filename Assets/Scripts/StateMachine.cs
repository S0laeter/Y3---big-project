using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected float time { get; set; }
    protected float fixedTime { get; set; }
    protected float lateTime { get; set; }

    public StateMachine stateMachine;

    public virtual void OnEnter(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    public virtual void OnUpdate()
    {
        time += Time.deltaTime;
    }
    public virtual void OnFixedUpdate()
    {
        fixedTime += Time.deltaTime;
    }
    public virtual void OnLateUpdate()
    {
        lateTime += Time.deltaTime;
    }

    public virtual void OnExit()
    {

    }

}

public class StateMachine : MonoBehaviour
{
    public string customName;

    private State mainStateType;

    public State currentState { get; private set; }
    private State nextState;

    private void Start()
    {
        SetNextStateToMain();
    }
    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextState != null)
        {
            SetState(nextState);
            nextState = null;
        }

        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    //change state locally
    private void SetState(State _newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = _newState;
        currentState.OnEnter(this);
    }

    //change the next state from other scripts
    public void SetNextState(State _newState)
    {
        if (_newState != null)
        {
            nextState = _newState;
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate();
        }
    }
    private void LateUpdate()
    {
        if (currentState != null)
        {
            currentState.OnLateUpdate();
        }
    }


    private void OnValidate()
    {
        if (mainStateType == null)
        {
            switch (customName)
            {
                case "player":
                    mainStateType = new PlayerIdleState();
                    break;
                case "boss":
                    break;
                default:
                    break;
            }
        }
    }

}
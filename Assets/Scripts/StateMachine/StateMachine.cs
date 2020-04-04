using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // Private Members
    private Dictionary<Type, State> stateDictionary = new Dictionary<Type, State>();
    private State currentState;
    private State queuedState;

    // Public Constructors
    public StateMachine(object controller, State[] states)
    {
        foreach (State state in states)
        {
            State instance = UnityEngine.Object.Instantiate(state);
            instance.Initialize(controller, this);
            stateDictionary.Add(instance.GetType(), instance);
            queuedState = queuedState ?? instance;
        }
    }

    // Public Methods
    public IEnumerator Start()
    {
        for( ; ; )
        {
            yield return Run();
        }
    }
    public IEnumerator Run()
    {
        if (currentState != queuedState)
        {
            yield return currentState?.Exit();
            currentState = queuedState;
            yield return currentState.Enter();
        }
        currentState.Run();
    }
    public void ChangeState<T>() where T : State
    {
        queuedState = stateDictionary[typeof(T)];
    }
}

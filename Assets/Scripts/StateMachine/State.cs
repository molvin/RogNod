using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    // Protected Members
    protected StateMachine stateMachine;
    protected object owner;

    // Public Methods
    public void Initialize(object owner, StateMachine stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }

    // Public Virtual Methods
    public virtual IEnumerator Enter() { yield return null; }
    public virtual IEnumerator Run() { yield return null; }
    public virtual IEnumerator Exit() { yield return null; }
}

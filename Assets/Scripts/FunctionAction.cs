using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class FunctionAction : ScriptableObject
{
    public string Name;
    public Image Icon;
    protected Entity actor;
    public Node Origin, Target;

    public virtual void Initialize(Entity actor) { this.actor = actor; }
    public abstract void AIDecision();
    public abstract IEnumerator Visualize();
    public abstract IEnumerator Act();
}

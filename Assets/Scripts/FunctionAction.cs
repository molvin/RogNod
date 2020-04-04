using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class FunctionAction : ScriptableObject
{
    public string Name;
    public Image Icon;
    protected Entity actor;
    public Node Origin, Target;

    public abstract void Initialize(Entity actor);
    public abstract IEnumerator Visualize();
    public abstract IEnumerator Act();
}

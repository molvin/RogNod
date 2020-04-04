using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class FunctionAction : ScriptableObject
{
    public string Name;
    public Image Icon;

    public abstract IEnumerator Visualize(GameObject actor, Node origin, Node target);
    public abstract IEnumerator Act(GameObject actor, Node origin, Node target);
}

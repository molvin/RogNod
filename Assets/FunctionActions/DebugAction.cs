using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DebugAction : FunctionAction
{
    public override IEnumerator Act()
    {
        Debug.Log("Acting");
        yield return null;
    }

    public override void AIDecision()
    {
        Debug.Log("AI Decision");
    }

    public override void Initialize(Entity actor)
    {
        Debug.Log("Initializing");
    }

    public override IEnumerator Visualize()
    {
        Debug.Log("Visualizing");
        yield return null;
    }
}

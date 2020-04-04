using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class HealSelfAction : FunctionAction
{
    public int heal;

    public override void Initialize(Entity actor)
    {
        base.Initialize(actor);
        Origin = actor.Node;
    }
    public override IEnumerator Act()
    {
        actor.Health += heal;
        yield return Visualize();
    }

    public override void AIDecision()
    {

    }

    public override void ResetVisualization()
    {
        //reset particleEffect
        Debug.Log("Reset ParticleEffect");
        if (visualization != null)
            Destroy(visualization);
    }
    private GameObject visualization;
    public override IEnumerator Visualize()
    {
        visualization = GameObject.CreatePrimitive(PrimitiveType.Cube);
        visualization.transform.position = Origin.transform.position;
        yield return new WaitForSeconds(0.6f);
        Destroy(visualization);
    }
}

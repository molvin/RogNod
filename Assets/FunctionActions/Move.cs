using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Move : FunctionAction
{
    public float ActLerpTime;
    public float VisualLerpTime;

    public override void Initialize(Entity actor)
    {
        this.actor = actor;
        this.Origin = actor.Node;
    }
    public override IEnumerator Act()
    {
        float time = 0;
        Origin.RemoveOccupant(actor);
        while (time / VisualLerpTime <= 1f)
        {
            actor.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / VisualLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        Target.AddOccupant(actor);
    }
    public override IEnumerator Visualize()
    {
        float time = 0;
        while(time / VisualLerpTime <= 0.5f)
        {
            actor.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / VisualLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        actor.transform.localPosition = Vector3.zero;
    }
}
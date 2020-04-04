using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Move : FunctionAction
{
    public float ActLerpTime;
    public float VisualLerpTime;
    public override IEnumerator Act(GameObject actor, Node origin, Node target)
    {
        float time = 0;
        origin.RemoveOccupant(actor);
        while (time / VisualLerpTime <= 1f)
        {
            actor.transform.position = Vector3.Lerp(origin.transform.position, target.transform.position, time / VisualLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        target.AddOccupant(actor);
    }

    public override IEnumerator Visualize(GameObject actor, Node origin, Node target)
    {
        float time = 0;
        while(time / VisualLerpTime <= 0.5f)
        {
            actor.transform.position = Vector3.Lerp(origin.transform.position, target.transform.position, time / VisualLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        actor.transform.localPosition = Vector3.zero;
    }
}

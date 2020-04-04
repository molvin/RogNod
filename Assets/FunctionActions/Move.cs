using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Move : FunctionAction
{
    public float ActLerpTime;
    public float VisualLerpTime;

    public Node origin;
    public Node target;

    public override void Initialize(Entity actor)
    {
        this.actor = actor;
        Enemy enemy = (Enemy)actor;

        Edge e = enemy.Node.Edges[Random.Range(0, enemy.Node.Edges.Count)];
        Node target = e.To;
        this.origin = enemy.Node;
        this.target = target;
    }
    public override IEnumerator Act()
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
    public override IEnumerator Visualize()
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
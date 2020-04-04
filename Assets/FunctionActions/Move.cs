using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        this.origin = actor.Node;
    }
    public override void AIDecision()
    {
        List<Node> adjacentNodes = origin.Edges.Select(e => e.To).ToList();
        Node bestNode = adjacentNodes.First();
        float bestDot = -1.0f;
        foreach (Node node in adjacentNodes)
        {
            float dot = Vector3.Dot((node.transform.position - origin.transform.position).normalized, 
                                    (GameLoop.PlayerNode.transform.position - origin.transform.position).normalized);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestNode = node;
            }
        }
        target = bestNode;
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
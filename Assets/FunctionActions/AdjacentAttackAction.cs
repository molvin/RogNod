using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class AdjacentAttackAction : FunctionAction
{
    public int damage;
    List<Node> adjacentNodes;

    public override void Initialize(Entity actor)
    {
        base.Initialize(actor);
        Origin = actor.Node;
        adjacentNodes = Origin.Edges.Select(e => e.To).ToList();
    }
    public override IEnumerator Act()
    {
        foreach (Node node in adjacentNodes)
        {
            for (int i = node.Occupants.Count - 1; i >= 0; i--)
            {
                Entity e = node.Occupants[i];
                e.Health -= damage;
            }
        }
        yield return null;
    }

    public override void AIDecision()
    {
    }

    public override void ResetVisualization()
    {
        adjacentNodes.ForEach(n => n.DemarkTile(actor is Enemy ? Node.Marker.Red : Node.Marker.Blue));
    }

    public override IEnumerator Visualize()
    {
        adjacentNodes.ForEach(n => n.MarkTile(actor is Enemy ? Node.Marker.Red : Node.Marker.Blue));
        yield return null;
    }
}

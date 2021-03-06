﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class SkipAttackAction : FunctionAction
{
    public int damage;
    public GameObject particleEffect;

    private List<Node> acceptableNode;
    public override void Initialize(Entity actor)
    {
        base.Initialize(actor);
        Origin = actor.Node;
        acceptableNode = Graph.Instance.Nodes.Where(n => Graph.Instance.ShortestPath(Origin, n).Count == 2).ToList();
    }
    public override IEnumerator Act()
    {
        if (!acceptableNode.Contains(Target))
            Target = acceptableNode[Random.Range(0, acceptableNode.Count)];
        for (int i = Target.Occupants.Count - 1; i >= 0; i--)
        {
            Entity e = Target.Occupants[i];
            e.Health -= damage;
        }

        Target.MarkTile(Node.Marker.Red);
        GameObject particle = Instantiate(particleEffect, Target.transform);
        yield return new WaitForSeconds(0.6f);
        Destroy(particle);
        Target.DemarkTile(Node.Marker.Red);
    }

    public override void AIDecision()
    {
        if (Graph.Instance.ShortestPath(Origin, GameLoop.PlayerNode).Count == 2)
            Target = GameLoop.PlayerNode;
        else
            Target = acceptableNode[Random.Range(0, acceptableNode.Count)];
    }

    public override void ResetVisualization()
    {
        Target.DemarkTile(acceptableNode.Contains(Target) ? Node.Marker.Blue : Node.Marker.Yellow);
    }
    
    public override IEnumerator Visualize()
    {
        Target.MarkTile(acceptableNode.Contains(Target) ? Node.Marker.Blue : Node.Marker.Yellow);
        yield return null;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class LongAction : FunctionAction
{
    public int damage;

    public override void Initialize(Entity actor)
    {
        base.Initialize(actor);
        Origin = actor.Node;
        Node best = null;
        float bestDist = float.MinValue;
        foreach (Node node in Origin.Edges.Select(e => e.To))
        {
            float distance = Vector3.Distance(Origin.transform.position, node.transform.position);
            if (distance > bestDist)
            {
                best = node;
                bestDist = distance;
            }
        }
        Target = best;
    }
    public override IEnumerator Act()
    {
        Debug.Log("Play ParticleEffect");
        for (int i = Target.Occupants.Count - 1; i >= 0; i--)
        {
            Entity e = Target.Occupants[i];
            Debug.Log("Damaging enemy: " + e.name);
            e.Health -= damage;
        }
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
        visualization.transform.position = Target.transform.position;
        yield return new WaitForSeconds(0.6f);
        Destroy(visualization);
    }
}

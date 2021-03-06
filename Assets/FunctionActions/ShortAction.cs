﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class ShortAction : FunctionAction
{
    public int damage;
    public AudioClip audioClip;
    public override void Initialize(Entity actor)
    {
        base.Initialize(actor);
        Origin = actor.Node;
        Node best = null;
        float bestDist = float.MaxValue;
        foreach (Node node in Origin.Edges.Select(e => e.To))
        {
            float distance = Vector3.Distance(Origin.transform.position, node.transform.position);
            if (distance < bestDist)
            {
                best = node;
                bestDist = distance;
            }
        }
        Target = best;
    }
    public override IEnumerator Act()
    {
        Visualize();
        Debug.Log("Play ParticleEffect");
        for (int i = Target.Occupants.Count - 1; i >= 0; i--)
        {
            Entity e = Target.Occupants[i];
            Debug.Log("Damaging enemy: " + e.name);
            e.Health -= damage;
        }

        GameLoop.Instance.audioSource.PlayOneShot(audioClip);

        ResetVisualization();
        yield return null;
        //yield return Visualize();
    }

    public override void AIDecision()
    {

    }

    public override void ResetVisualization()
    {
        //reset particleEffect
        if (actor is Enemy)
            Target.DemarkTile(Node.Marker.Red);
        else
            Target.DemarkTile(Node.Marker.Yellow);
        /*  if (visualization != null)
            Destroy(visualization);
    */
    }
    //private GameObject visualization;
    public override IEnumerator Visualize()
    {
        if (actor is Enemy)
            Target.MarkTile(Node.Marker.Red);
        else
            Target.MarkTile(Node.Marker.Yellow);
        /*
        visualization = GameObject.CreatePrimitive(PrimitiveType.Cube);
        visualization.transform.position = Target.transform.position;*/
        yield return null;
    }
}

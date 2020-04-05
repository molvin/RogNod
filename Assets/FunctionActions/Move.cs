using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class Move : FunctionAction
{
    public float ActLerpTime;
    public float VisualLerpTime;
    public int EnemyMoveDamage;

    private GameObject visualization;

    public override void Initialize(Entity actor)
    {
        this.actor = actor;
        this.Origin = actor.Node;
    }
    public override void AIDecision()
    {
        List<Node> adjacentNodes = Origin.Edges.Select(e => e.To).ToList();
        Node bestNode = adjacentNodes.First();
        float bestDot = -1.0f;
        foreach (Node node in adjacentNodes)
        {
            float dot = Vector3.Dot((node.transform.position - Origin.transform.position).normalized, 
                                    (GameLoop.PlayerNode.transform.position - Origin.transform.position).normalized);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestNode = node;
            }
        }
        Target = bestNode;
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
        DamagePlayer();
        Target.AddOccupant(actor);
    }
    private void DamagePlayer()
    {
        foreach (Entity entity in Target.Occupants)
        {
            if (entity is Player)
                entity.Health -= EnemyMoveDamage;
        }
    }
    public override IEnumerator Visualize()
    {
        if (visualization != null)
            Destroy(visualization);

        visualization = Instantiate(actor.gameObject);

        float time = 0;
        while(time / VisualLerpTime <= 1f)
        {
            if (visualization == null)
                break;
            visualization.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / VisualLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        if (visualization != null)
            Destroy(visualization);
    }

    public override void ResetVisualization()
    {
        if (visualization != null)
            Destroy(visualization);
    }
}
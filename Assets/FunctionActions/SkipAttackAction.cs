using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu()]
public class SkipAttackAction : FunctionAction
{
    public int damage;
    public Material acceptMat;
    public Material denyMat;

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
        if (Graph.Instance.ShortestPath(Origin, GameLoop.PlayerNode).Count == 2)
            Target = GameLoop.PlayerNode;
        else
            Target = acceptableNode[Random.Range(0, acceptableNode.Count)];
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
        visualization.transform.GetComponent<MeshRenderer>().sharedMaterial = acceptableNode.Contains(Target) ? acceptMat : denyMat;
        yield return new WaitForSeconds(0.6f);
        Destroy(visualization);
    }
}

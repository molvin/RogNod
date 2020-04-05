using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu()]
public class Laserbeam : FunctionAction
{
    public float AllowedDotForNextNode;
    public int Damage;
    public int MaxRange;
    public GameObject LightningStrike;

    public override void Initialize(Entity actor)
    {
        base.Initialize(actor);
        Origin = actor.Node;
    }

    public override IEnumerator Act() {
        List<Node> hitnodes = GetNodes();
        foreach (Node n in hitnodes)
        {
            for(int i = n.Occupants.Count; i>=0;i--)
            {
                n.Occupants[i].Health -= Damage;
                Instantiate(LightningStrike, new Vector3(n.transform.position.x, n.transform.position.y, 5), Quaternion.identity, null);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override IEnumerator Visualize()
    {
        List<Node> hitnodes = GetNodes();
        foreach (Node n in hitnodes)
        {
            n.MarkTile(Node.Marker.Red);
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    public override void ResetVisualization()
    {
        List<Node> hitnodes = GetNodes();
        foreach (Node n in hitnodes)
            n.DemarkTile(Node.Marker.Red);
    }

    private Node BestNode(Node current, Vector2 AimDirection)
    {
        Node bestNode = null;
        float best = 0f;
        foreach (Node n in current.Edges.Select((e) => e.To))
        {
            float dot = Vector2.Dot(n.transform.position - current.transform.position, AimDirection);
            if (dot > best)
            {
                best = dot;
                bestNode = n;
            }
        }
        return bestNode;
    }

    private List<Node> GetNodes()
    {
        List<Node> hitnodes = new List<Node>();
        Vector2 PlayerDirection = Target.transform.position - Origin.transform.position;
        hitnodes.Add(Target);

        Node current = Target;
        for (int i = 0; i < MaxRange; i++)
        {
            Node best = BestNode(current, PlayerDirection);
            float dot = Vector2.Dot(best.transform.position - current.transform.position, PlayerDirection);
            if (dot > AllowedDotForNextNode && best != null)
            {
                hitnodes.Add(best);
                current = best;
            }
        }
        return hitnodes;
    }

    public override void AIDecision() {

        Vector2 aimDirection = GameLoop.PlayerNode.transform.position - Origin.transform.position;
        Target = BestNode(Origin, aimDirection);
    }
}

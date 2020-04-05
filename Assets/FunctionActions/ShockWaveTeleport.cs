using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Targets any Unoccupied Node.
[CreateAssetMenu()]
public class ShockWaveTeleport : FunctionAction
{
    public int damage;
    public float actLerpTime;
    public int maxRange;

    public override void Initialize(Entity actor)
    {
        this.actor = actor;
        this.Origin = actor.Node;
    }
    public override void AIDecision()
    {
        Target = GameLoop.PlayerNode;
    }
    public override IEnumerator Act()
    {
        List<Node> path = Graph.Instance.ShortestPath(Origin, Target);
        Debug.Log(path.Count);

        //damages everyone at current Node
        Origin.RemoveOccupant(actor);
        for(int d = Origin.Occupants.Count -1; d >= 0; d--)
        {
                Origin.Occupants[d].Health -= damage;
        }

        for (int i = 0; i < path.Count; i++) {
            Node currentNode = i > 0 ? path[i - 1] : actor.Node;
            Node nextNode = path[i];
            float time = 0;
          //lerps position to nextNode
            while (time / (actLerpTime / path.Count)  <= 1f)
            {
                actor.transform.position = Vector3.Lerp(currentNode.transform.position, nextNode.transform.position, time / (actLerpTime / path.Count));
                time += Time.deltaTime;
                yield return null;
            }

            //Iterates through nextNodes Occupants and damages
            for (int j = nextNode.Occupants.Count -1; j >= 0; j--)
            {
                if(nextNode.Occupants[j] != actor)
                {
                    nextNode.Occupants[j].Health -= damage;
                }
            }
        }
        Target.AddOccupant(actor);        
    }
    public override IEnumerator Visualize()
    {

        List<Node> path = Graph.Instance.ShortestPath(actor.Node, Target);
        
        foreach(Node n in path)
        {
            n.MarkTileRed();
        }

        for (int i = 0; i < path.Count; i++)
        {
            Node currentNode = i > 0 ? path[i - 1] : actor.Node;
            Node nextNode = path[i];
            float time = 0;
            //lerps position to nextNode
            while (time / (actLerpTime / path.Count) <= 1f)
            {
                actor.transform.position = Vector3.Lerp(currentNode.transform.position, nextNode.transform.position, time / (actLerpTime / path.Count));
                time += Time.deltaTime;
                yield return null;
            }
        }
        ResetVisualization();
    }

    public override void ResetVisualization()
    {
        foreach (Node n in Graph.Instance.ShortestPath(Origin, Target))
        {
            n.ResetTileGrapic();
        }
        actor.transform.localPosition = Vector3.zero;
    }
}

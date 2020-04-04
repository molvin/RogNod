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
        Target = GameLoop.PlayerNode;
    }
    public override void AIDecision()
    {

    }
    public override IEnumerator Act()
    {
        Origin = actor.Node;
        List<Node> path = Graph.Instance.ShortestPath(actor.Node, Target);
        //damages everyone at current Node
        Origin.RemoveOccupant(actor);
        for(int d = Origin.Occupants.Count; d > 0; d--)
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
            for (int j = nextNode.Occupants.Count; j > 0; j++)
            {
                if(nextNode.Occupants[j] != actor)
                {
                    nextNode.Occupants[j].Health -= damage;
                }
            }
        }
        Target.AddOccupant(actor);

        //next node that is travelled to

        //while true.
        //set nextNode to next Node in queue
        //Lerp to nextNode
        //Check if lerpAlpha >= 1;
        //if nextNode == target, break. else Damage any other entity on this tile, and return.


        
    }
    public override IEnumerator Visualize()
    {

        List<Node> path = Graph.Instance.ShortestPath(actor.Node, Target);
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
            //Iterates through nextNodes Occupants and damages
            ResetVisualization();
        }
    }

    public override void ResetVisualization()
    {
        actor.transform.localPosition = Vector3.zero;
    }
}

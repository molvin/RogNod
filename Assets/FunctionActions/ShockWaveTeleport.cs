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
    private GameObject visualizeChar;


    public AudioClip audioClip;

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
        if (visualizeChar != null)
        {
            ResetVisualization();
        }

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
           //AudioSource.

            //Iterates through nextNodes Occupants and damages
            for (int j = nextNode.Occupants.Count -1; j >= 0; j--)
            {
                bool playedAudio = false;
                if(nextNode.Occupants[j] != actor)
                {
                    nextNode.Occupants[j].Health -= damage;
                    if(!playedAudio)
                        GameLoop.Instance.audioSource.PlayOneShot(audioClip);
                    playedAudio = true;
                }
            }
        }
        ResetVisualization();
        Target.AddOccupant(actor);        
    }
    public override IEnumerator Visualize()
    {
        if (visualizeChar != null)
        {
            ResetVisualization();
        }
        this.Origin = actor.Node;

        List<Node> path = Graph.Instance.ShortestPath(actor.Node, Target);
        foreach (Node n in path)
        {
            if (actor is Enemy)
                n.MarkTile(Node.Marker.Red);
            else
                n.MarkTile(Node.Marker.Yellow);
        }

        visualizeChar = Instantiate(actor.gameObject);
        SpriteRenderer renderer = visualizeChar.GetComponentInChildren<SpriteRenderer>();
        Color c = renderer.color;
        c.a = 0.65f;
        renderer.color = c;

        for (int i = 0; i < path.Count; i++)
        {
            Node currentNode = i > 0 ? path[i - 1] : actor.Node;
            Node nextNode = path[i];
            float time = 0;
            //lerps position to nextNode
            while (time / (actLerpTime / path.Count) <= 1f)
            {
                visualizeChar.transform.position = Vector3.Lerp(currentNode.transform.position, nextNode.transform.position, time / (actLerpTime / path.Count));
                time += Time.deltaTime;
                yield return null;
            }
        }
       // ResetVisualization();
    }

    public override void ResetVisualization()
    {
        if (visualizeChar != null)
        {
            Destroy(visualizeChar);
        }
        foreach (Node n in Graph.Instance.ShortestPath(actor.Node, Target))
        {
            if (actor is Enemy)
                n.DemarkTile(Node.Marker.Red);
            else
                n.DemarkTile(Node.Marker.Yellow);
        }
        actor.transform.localPosition = Vector3.zero;
    }
}

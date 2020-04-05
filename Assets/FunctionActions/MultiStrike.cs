using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MultiStrike : FunctionAction
{
    public int damage;
    public GameObject particalEffect;
    private List<Node> Targets = new List<Node>();
    public int Count;
    public AudioClip audioClip;

    public override void AIDecision()
    {
        Target = GameLoop.Instance.Player.Node;
        Targets.Add(Target);
        for(int i = 0; i < Count - 1; i++)
        {
            Node n = Graph.GetRandomNode();
            int j = 0;
            while (Targets.Contains(n) && j++ < 50)
                n = Graph.GetRandomNode();
            Targets.Add(n);
        }
    }
    public override IEnumerator Visualize()
    {
        foreach(Node t in Targets)
            t.MarkTile(Node.Marker.Red);
        yield return null;
    }
    public override void ResetVisualization()
    {
        foreach (Node t in Targets)
            Target.DemarkTile(Node.Marker.Red);
        
    }
    // Update is called once per frame
    public override IEnumerator Act()
    {
        Debug.Log("Play ParticleEffect");
        GameLoop.Instance.audioSource.PlayOneShot(audioClip);

        foreach (Node t in Targets)
        {
            GameObject particle = Instantiate(particalEffect, t.transform);

        }
        foreach(Node t in Targets)
        {
            for (int i = t.Occupants.Count - 1; i >= 0; i--)
            {

                Entity e = t.Occupants[i];

                //Checks if target entity is of other type;
                if (actor is Enemy && e is Player || actor is Player && e is Enemy)
                {
                    e.Health -= damage;
                }
            }
        }

       
        yield return null;
    }
}

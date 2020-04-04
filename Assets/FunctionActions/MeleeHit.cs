using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class MeleeHit : FunctionAction
{
    public int damage;
    // Start is called before the first frame update
    public override IEnumerator Act()
    {
        Debug.Log("Play ParticleEffect");
        for (int i = Target.Occupants.Count - 1; i >= 0; i--)
        
        {
            Entity e = Target.Occupants[i];
            Debug.Log("MeleeDamaging enemy: " + e.name);
            e.Health -= damage;
        }

        yield return null;
    }


    public override void AIDecision()
    {
        foreach(Edge e in Origin.Edges)
        {
            if (e.To.Equals(GameLoop.PlayerNode))
                Target = e.To;
        }
    }

    public override void ResetVisualization()
    {

    }

    public override IEnumerator Visualize()
    {
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class LightningStrike : FunctionAction
{

    public int damage;
    public GameObject particalEffect;
    public AudioClip audioClip;

    public override void AIDecision()
    {
        Target = GameLoop.Instance.Player.Node;
    }
    public override IEnumerator Visualize()
    {
        Target.MarkTile(Node.Marker.Red);
        yield return null;
    }
    public override void ResetVisualization()
    {
        Target.DemarkTile(Node.Marker.Red);

    }
    // Update is called once per frame
    public override IEnumerator Act()
    {
        GameLoop.Instance.audioSource.PlayOneShot(audioClip);

        GameObject particle = Instantiate(particalEffect, Target.transform);

        for (int i = Target.Occupants.Count - 1; i >= 0; i--)
        {
            
            Entity e = Target.Occupants[i];

            //Checks if target entity is of other type;
            if(actor is Enemy && e is Player || actor is Player && e is Enemy)
            {
                e.Health -= damage;
            }
        }
        yield return null;
    }
}

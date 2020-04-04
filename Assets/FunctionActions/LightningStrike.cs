using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class LightningStrike : FunctionAction
{

    public int damage;

    public override void AIDecision()
    {
        Target = GameLoop.Instance.Player.Node;
    }
    public override IEnumerator Visualize()
    {
        Debug.Log("Play LightningStrike ParticleEffect here");
        yield return null;
        ResetVisualization();
    }
    public override void ResetVisualization()
    {
        //reset particleEffect
        Debug.Log("Reset ParticleEffect");
    }
    // Update is called once per frame
    public override IEnumerator Act()
    {
        Debug.Log("Play ParticleEffect");
       foreach(Entity e in Target.Occupants){
            e.Health -= damage;
        }
        yield return null;
    }

}

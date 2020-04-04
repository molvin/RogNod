using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Targets any Unoccupied Node.
[CreateAssetMenu()]
public class ShockWaveTeleport : FunctionAction
{
    public float Damage;
    public float actLerpTime;
    public Node origin;
    public Node target;

    public override void Initialize(Entity actor)
    {
        throw new System.NotImplementedException();
    }
    public override void AIDecision()
    {
        throw new System.NotImplementedException();
    }
    public override IEnumerator Act()
    {
        float time = 0;
        Node nextNode; //next node that is travelled to
        //while true.
        //set nextNode to next Node in queue
        //Lerp to nextNode
        //Check if lerpAlpha >= 1;
        //if nextNode == target, break. else Damage any other entity on this tile, and return.
        

        yield return null;
    }
    public override IEnumerator Visualize()
    {
        float time = 0;
        while (time / actLerpTime <= 1f)
        {
            actor.transform.position = Vector3.Lerp(origin.transform.position, target.transform.position, time / actLerpTime);
            actor.transform.position = Vector3.Lerp(origin.transform.position, target.transform.position, time / actLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        target.AddOccupant(actor);
        
    }

}

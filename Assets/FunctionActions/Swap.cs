using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//swaps position of actor and target entity
[CreateAssetMenu()]
public class Swap : FunctionAction
{ 
    public float actLerpTime;
    public Entity targetSwapEntity;
    public Node origin;
    public Node target;
    
    // Start is called before the first frame update
    public override void Initialize(Entity actor)
    {
        this.actor = actor;
        //Set origin to actor's current Node
        origin = actor.Node;
        
        target = targetSwapEntity.Node;
        //set target to target node where swaptarget is positioned

    }
    public override void AIDecision()
    {
        throw new System.NotImplementedException();
    }
    public override IEnumerator Act()
    {
        float time = 0;
        origin.RemoveOccupant(actor);
        target.RemoveOccupant(targetSwapEntity);
        while(time / actLerpTime <= 1f)
        {
            actor.transform.position = Vector3.Lerp(origin.transform.position, target.transform.position, time / actLerpTime);
            targetSwapEntity.transform.position = Vector3.Lerp(target.transform.position, origin.transform.position, time / actLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        origin.AddOccupant(targetSwapEntity);
        target.AddOccupant(actor);
    }

    public override IEnumerator Visualize()
    {
        float time = 0;
        while (time / actLerpTime <= 0.5f)
        {
            actor.transform.position = Vector3.Lerp(origin.transform.position, target.transform.position, time / actLerpTime);
            targetSwapEntity.transform.position = Vector3.Lerp(target.transform.position, origin.transform.position, time / actLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        actor.transform.localPosition = Vector3.zero;
    }

    public override void ResetVisualization()
    {
        throw new System.NotImplementedException();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//swaps position of actor and target entity
[CreateAssetMenu()]
public class Swap : FunctionAction
{
    public float actLerpTime;
    public List<Entity> targetSwapEntity;

    // Start is called before the first frame update
    public override void Initialize(Entity actor)
    {
        this.actor = actor;
        //Set origin to actor's current Node
        Origin = actor.Node;
        targetSwapEntity = Target.Occupants;

        Target = targetSwapEntity[0].Node;
        //set target to target node where swaptarget is positioned

    }
    public override void AIDecision()
    {
        throw new System.NotImplementedException();
    }
    public override IEnumerator Act()
    {
        float time = 0;
        //save Entities on both target and originNodes
        List<Entity> originEntitiesTemp = Origin.Occupants; 
        List<Entity> targetEntitiesTemp = Target.Occupants;

        //Remove them from the nodes
        foreach(Entity e in Origin.Occupants)
        {
            Origin.RemoveOccupant(e);
        }

        foreach(Entity e in Target.Occupants)
        {
            Target.RemoveOccupant(e);
        }
        while(time / actLerpTime <= 1f)
        {
            //Move from origin to target
            foreach(Entity e in originEntitiesTemp)
            {
                e.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / actLerpTime);
            }
            //move from target to origin
            foreach (Entity e in targetEntitiesTemp)
            {
                e.transform.position = Vector3.Lerp(Target.transform.position, Origin.transform.position, time / actLerpTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
        foreach (Entity e in originEntitiesTemp)
        {
            Target.AddOccupant(e);
        }
        foreach (Entity e in targetEntitiesTemp)
        {
            Origin.AddOccupant(e);
        }
    }

    public override IEnumerator Visualize()
    {
        float time = 0;

        while (time / actLerpTime <= 0.5f)
        {
            foreach (Enemy e in Origin.Occupants)
            { 
                e.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / actLerpTime);
            }
            foreach (Enemy e in Target.Occupants)
            {
                e.transform.position = Vector3.Lerp(Target.transform.position, Origin.transform.position, time / actLerpTime);
            }
            time += Time.deltaTime;
            yield return null;
        }
        ResetVisualization();
    }

    public override void ResetVisualization()
    {
        foreach (Enemy e in Origin.Occupants)
            e.transform.localPosition = Vector3.zero;
        foreach (Enemy e in Target.Occupants)
            e.transform.localPosition = Vector3.zero;
    }
}

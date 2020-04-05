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
        //set target to target node where swaptarget is positioned

    }
    public override void AIDecision()
    {
        throw new System.NotImplementedException();
    }
    public override IEnumerator Act()
    {
        float time = 0;

        while (time / actLerpTime <= 1f)
        {
            //Move from origin to target
            foreach (Entity e in Origin.Occupants)
            {
                e.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / actLerpTime);
            }
            //move from target to origin
            foreach (Entity e in Target.Occupants)
            {
                e.transform.position = Vector3.Lerp(Target.transform.position, Origin.transform.position, time / actLerpTime);
            }
            time += Time.deltaTime;
            yield return null;
        }

        List<Entity> OriginOccupantTest = Origin.Occupants.GetRange(0, Origin.Occupants.Count);
        for (int i = Origin.Occupants.Count - 1; i >= 0; i--)
        {
            Entity e = Origin.Occupants[i];

            //stuns any entity of enemy type
            if (actor != e)
            {
                e.Stunned = true;
            }
            Origin.RemoveOccupant(e);
        }
        //Add Target occupants to list
        List<Entity> TargetOccupantTest = Target.Occupants.GetRange(0, Target.Occupants.Count);

        for (int i = Target.Occupants.Count - 1; i >= 0; i--)
        {
            Entity e = Target.Occupants[i];
            //stuns any entity of enemy type
            if (actor != e)
            {
                e.Stunned = true;
            }

            Target.RemoveOccupant(e);
        }

        foreach (Entity e in TargetOccupantTest)
        {
            Origin.AddOccupant(e);
        }

        foreach (Entity e in OriginOccupantTest)
        {
            Target.AddOccupant(e);
        }
    }

    public override IEnumerator Visualize()
    {
        float time = 0;

        while (time / actLerpTime <= 1f)
        {
            foreach (Entity e in Origin.Occupants)
            {
                e.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / actLerpTime);
            }
            foreach (Entity e in Target.Occupants)
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
        foreach (Entity e in Origin.Occupants)
            e.transform.localPosition = Vector3.zero;
        foreach (Entity e in Target.Occupants)
            e.transform.localPosition = Vector3.zero;
    }
}

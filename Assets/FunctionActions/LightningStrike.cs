﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class LightningStrike : FunctionAction
{

    public int damage;
    public GameObject particalEffect; 

    public override void AIDecision()
    {
        Target = GameLoop.Instance.Player.Node;
    }
    public override IEnumerator Visualize()
    {
        Debug.Log("Play LightningStrike ParticleEffect here");
        GameObject particle = Instantiate(particalEffect, Target.transform);
        
        //particle.transform.LookAt(Target.transform, Vector2.up);
        
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

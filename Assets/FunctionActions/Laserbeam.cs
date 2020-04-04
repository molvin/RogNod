using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu()]
public class Laserbeam : FunctionAction
{
    public int damage;

    public override IEnumerator Act() {
        Debug.Log("Play ParticleEffect");
        for (int i = Target.Occupants.Count - 1; i >= 0; i--) {
            Entity e = Target.Occupants[i];
            Debug.Log("Damaging enemy: " + e.name);
            e.Health -= damage;
        }
        yield return null;
    }



    public override IEnumerator Visualize() {

        for(int i = 0; i<5; i++) {
            List<Node> adjacentNodes = Origin.Edges.Select(e => e.To).ToList();
            Node bestNode = adjacentNodes.First();
            float bestDot = -1.0f;
            foreach (Node node in adjacentNodes) {
                float dot = Vector3.Dot((node.transform.position - Origin.transform.position).normalized,
                                        (Target.transform.position - Origin.transform.position).normalized);
                if (dot > bestDot) {
                    bestDot = dot;
                    bestNode = node;
                }
            }
            Target = bestNode;
            Act();
            yield return new WaitForSeconds(0.1f);
        }



        yield return null;
    }

    public override void ResetVisualization() {
        throw new System.NotImplementedException();
    }


    public override void AIDecision() {
        throw new System.NotImplementedException();
    }


}

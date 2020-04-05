using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class Move : FunctionAction
{
    public float ActLerpTime;
    public float VisualLerpTime;
    public int EnemyMoveDamage;
    public Sprite arrow;

    private GameObject visualizeChar;
    //private GameObject visualizeArrow;

    public override void Initialize(Entity actor)
    {
        this.actor = actor;
        this.Origin = actor.Node;
    }
    public override void AIDecision()
    {
        List<Node> adjacentNodes = Origin.Edges.Select(e => e.To).ToList();
        Node bestNode = adjacentNodes.First();
        float bestDot = -1.0f;
        foreach (Node node in adjacentNodes)
        {
            float dot = Vector3.Dot((node.transform.position - Origin.transform.position).normalized, 
                                    (GameLoop.PlayerNode.transform.position - Origin.transform.position).normalized);
            if (dot > bestDot)
            {
                bestDot = dot;
                bestNode = node;
            }
        }
        Target = bestNode;
    }
    public override IEnumerator Act()
    {
        if (visualizeChar != null)
        {
            Destroy(visualizeChar);
            //Destroy(visualizeArrow);
        }
        float time = 0;
        Origin.RemoveOccupant(actor);
        while (time / VisualLerpTime <= 1f)
        {
            actor.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / VisualLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
        DamagePlayer();
        Target.AddOccupant(actor);
    }
    private void DamagePlayer()
    {
        foreach (Entity entity in Target.Occupants)
        {
            if (entity is Player)
                entity.Health -= EnemyMoveDamage;
        }
    }
    public override IEnumerator Visualize()
    {
        if (visualizeChar != null)
        {
            Destroy(visualizeChar);
            //Destroy(visualizeArrow);
        }

        visualizeChar = Instantiate(actor.gameObject);
        SpriteRenderer renderer = visualizeChar.GetComponentInChildren<SpriteRenderer>();
        Destroy(visualizeChar.GetComponentInChildren<TextMeshProUGUI>());
        Destroy(visualizeChar.GetComponentInChildren<Image>());
        Color c = renderer.color;
        c.a = 0.65f;
        renderer.color = c;

        /*visualizeArrow = new GameObject();
        SpriteRenderer ren = visualizeArrow.AddComponent<SpriteRenderer>();
        ren.sprite = arrow;
        ren.color = Color.green;
        visualizeArrow.transform.position = (Origin.transform.position * 0.7f + Target.transform.position * 0.3f);
        if (Target.transform.position.x < Origin.transform.position.x)
            ren.flipX = true;
        */
        float time = 0;
        while(time / VisualLerpTime <= 1f)
        {
            if (visualizeChar == null)
                break;
            visualizeChar.transform.position = Vector3.Lerp(Origin.transform.position, Target.transform.position, time / VisualLerpTime);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public override void ResetVisualization()
    {
        if (visualizeChar != null)
        {
            Destroy(visualizeChar);
            //Destroy(visualizeArrow);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : Entity
{
    [SerializeField] private List<Card> actionDeck;
    [HideInInspector] public Card card;
    [HideInInspector] public FunctionAction action;
    public TextMeshProUGUI HpText;

    public override int Health { get => health; set
        {
            health = value;
            HpText.text = Mathf.Max(health, 0).ToString();
            if (health <= 0)
            {
                Node.RemoveOccupant(this);
                action?.ResetVisualization();
                GameLoop.Instance.enemies.Remove(this);
                Destroy(gameObject);
            }
        }
    }

    public FunctionAction PickAction()
    {
        card = actionDeck[Random.Range(0, actionDeck.Count)];
        action = Instantiate(card.getAction());
        action.Initialize(this);
        action.AIDecision();
        return action;
    }
    public IEnumerator StubVisualize()
    {
        float time = 0;
        while (time < 1f)
        {
            Debug.Log("Visualizing: " + transform.name);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
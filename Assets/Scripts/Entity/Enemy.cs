﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private List<FunctionAction> actionDeck;
    [HideInInspector] public FunctionAction action;

    public override int Health { get => health; set
        {
            health = value;
            if (health <= 0)
            {
                Node.RemoveOccupant(this);
                GameLoop.Instance.enemies.Remove(this);
                Destroy(gameObject);
            }
        }
    }

    public FunctionAction PickAction()
    {
        action = Instantiate(actionDeck[Random.Range(0, actionDeck.Count)]);
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
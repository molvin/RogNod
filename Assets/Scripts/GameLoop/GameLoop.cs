﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // Public Members
    public State[] states;
    public List<Enemy> enemies;
    public List<Enemy> EnemyPrefabs;

    public int EnemyCount;

    public Player PlayerPrefab;
    public Player Player;

    // Private Members
    private StateMachine stateMachine;

    // Public Methods
    private void Awake()
    {
        stateMachine = new StateMachine(this, states);
        
        for(int i = 0; i < EnemyCount; i++)
        {
            Enemy prefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)];
            Enemy instance = Instantiate(prefab);
            Node node = Graph.GetUnoccupiedRandomNode();
            node.AddOccupant(instance);
            enemies.Add(instance);
        }

        Player = Instantiate(PlayerPrefab);
        Node n = Graph.GetUnoccupiedRandomNode();
        n.AddOccupant(Player);
    }
    private void Start()
    {
        StartCoroutine(stateMachine.Start());
    }
}

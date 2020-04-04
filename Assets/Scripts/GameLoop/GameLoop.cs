using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // Public Members
    public State[] states;
    public List<Enemy> enemies;
    public List<Enemy> EnemyPrefabs;

    public int EnemyCount;

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
            node.AddOccupant(instance.gameObject);
            instance.Node = node;
            enemies.Add(instance);
        }
    }
    private void Start()
    {
        StartCoroutine(stateMachine.Start());
    }
}

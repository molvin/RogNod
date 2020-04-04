using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public static GameLoop Instance;
    // Public Members
    public State[] states;
    public List<Enemy> enemies;
    public List<Enemy> EnemyPrefabs;

    public int EnemyCount;

    public Player PlayerPrefab;
    public Player Player;

    // Private Members
    private StateMachine stateMachine;

    // Properties
    public PlayerState PlayerState => stateMachine.GetState<PlayerState>() as PlayerState;
    public static Node PlayerNode => Instance.Player.Node;

    // Public Methods
    private void Awake()
    {
        Instance = this;
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

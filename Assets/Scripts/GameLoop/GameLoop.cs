using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    // Public Members
    public State[] states;
    public List<Enemy> enemies;

    // Private Members
    private StateMachine stateMachine;

    // Public Methods
    private void Awake()
    {
        stateMachine = new StateMachine(this, states);
    }
    private void Start()
    {
        StartCoroutine(stateMachine.Start());
    }
}

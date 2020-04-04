using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActiontTest : MonoBehaviour
{
    public Node start;
    public Node target;
    public void AddAction(FunctionAction action)
    {
        GameLoop.Instance.PlayerState.SetAction(action);
    }
    
    public void DebugPath()
    {
        List<Node> path = Graph.Instance.ShortestPath(start, target);
        foreach (Node p in path)
        {
            Debug.Log(p.name);
        }
    }
}

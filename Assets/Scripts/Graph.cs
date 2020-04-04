using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public float SpawnRadius;
    public float MinDistance;
    public int NodeCount;
    public Node NodePrefab;
    public Edge EdgePrefab;
    private List<Node> nodes = new List<Node>();
    private List<Edge> edges = new List<Edge>();

    private void Start()
    {
        Generate(NodeCount);                
    }
    private void Generate(int v)
    {
        while(nodes.Count < v)
        {
            //Random position
            Vector2 position = Random.insideUnitCircle * SpawnRadius;
            //Check too close
            bool s = true;
            foreach (Node n in nodes)
            {
                if(Vector2.Distance(n.transform.position, position) < MinDistance)
                {
                    s = false;
                    break;
                }
            }
            if (!s)
                continue;
            //Create node
            Node node = Instantiate(NodePrefab, position, Quaternion.identity);
            //Make connections


            nodes.Add(node);                        
        }
    }

}

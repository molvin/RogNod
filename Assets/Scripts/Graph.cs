using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public float SpawnRadius;
    public float MinDistance;
    public float MaxEdgeLength;
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
            Node newNode = Instantiate(NodePrefab, position, Quaternion.identity);
            newNode.name = $"Node {nodes.Count}";
            nodes.Add(newNode);                        
        }
        foreach(Node node in nodes)
        {
            //Create connection to each node
            foreach (Node n in nodes)
            {
                if (node == n)
                    continue;
                bool fail = false;
                foreach(Edge e in edges)
                {
                    if (e.To == n || e.From == n || e.To == node || e.From == n)
                        continue;
                    //If vec overlaps e, dissallow
                    Vector2 intersect;
                    if(LineSegmentsIntersection(node.transform.position, n.transform.position, e.To.transform.position, e.From.transform.position, out intersect))
                    {
                        if((intersect - (Vector2)node.transform.position).magnitude > 0.1)
                        {
                            fail = true;
                            break;
                        }
                    }
                }
                if(!fail)
                {
                    Edge e = Instantiate(EdgePrefab, node.transform.position, Quaternion.identity);
                    e.From = node;
                    e.To = n;
                    edges.Add(e);
                    node.Edges.Add(e);
                    n.Edges.Add(e);
                }
            }
        }
        edges = edges.OrderBy(x => Random.value).ToList();
        for(int i = edges.Count - 1; i >= 0; --i)
        {
            Edge e = edges[i];
            //if edge is too long
            if(Vector2.Distance(e.To.transform.position, e.From.transform.position) > MaxEdgeLength && e.To.Edges.Count > 1 && e.From.Edges.Count > 1)
            {
                Debug.Log("Removed based on length");
                edges.RemoveAt(i);
                e.To.Edges.Remove(e);
                e.From.Edges.Remove(e);
                Destroy(e.gameObject);
                continue;
            }

        }
    }

    public static bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if (d == 0.0f)
        {
            return false;
        }

        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        intersection.x = p1.x + u * (p2.x - p1.x);
        intersection.y = p1.y + u * (p2.y - p1.y);

        return true;
    }


}

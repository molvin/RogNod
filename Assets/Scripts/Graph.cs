﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Graph : MonoBehaviour
{
    public static Graph Instance;
    public float SpawnRadius;
    public float MinDistance;
    public float MaxEdgeLength;
    public int NodeCount;
    public float YTiltAmount;
    public LayerMask NodeLayer;
    public Node NodePrefab;
    public Edge EdgePrefab;
    private List<Node> nodes = new List<Node>();
    private List<Edge> edges = new List<Edge>();

    public int CountForRandomRemove;
    public float RemoveChance;

    public List<Node> Nodes => nodes.GetRange(0, nodes.Count);

    private void Awake()
    {
        Instance = this;
        Generate(NodeCount + Persistance.Instance.NodeCountIncrease * Persistance.Instance.Round);                
    }
    private void Generate(int v)
    {
        int escape = 100;
        while(nodes.Count < v || escape < 0)
        {
            escape--;
            //Random position
            Vector2 position = new Vector2 ((Random.value-0.5f) * SpawnRadius * YTiltAmount, (Random.value - 0.5f) * SpawnRadius);
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

                RaycastHit[] hits = Physics.RaycastAll(node.transform.position, (n.transform.position - node.transform.position).normalized, (n.transform.position - node.transform.position).magnitude, NodeLayer);
                foreach(RaycastHit h in hits)
                {
                    if (h.collider.gameObject != n.gameObject && h.collider.gameObject != node.gameObject)
                        fail = true;
                }

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
                    //n.Edges.Add(e);
                }
            }
        }
        edges = edges.OrderBy(x => Random.value).ToList();
        List<Edge> to_ignore = new List<Edge>();
        for(int i = edges.Count - 1; i >= 0; --i)
        {
            Edge e = edges[i];
            //if edge is too long
            if(Vector2.Distance(e.To.transform.position, e.From.transform.position) > MaxEdgeLength && e.To.Edges.Count > 1 && e.From.Edges.Count > 1)
            {
                to_ignore.Add(e);

                edges.RemoveAt(i);
                e.From.Edges.Remove(e);
                for (int j = e.To.Edges.Count - 1; j >= 0; j--)
                {
                    Edge et = e.To.Edges[j];
                    if (et.To == e.From && et.From == e.To)
                    {
                        Edge e1 = e.To.Edges[j];

                        e.To.Edges.RemoveAt(j);
                        //edges.Remove(e1);
                        to_ignore.Add(e1);

                        Destroy(et.gameObject);
                        break;
                    }                    
                }
                Destroy(e.gameObject);
                continue;
            }
        }
        foreach (Edge e in to_ignore)
            edges.Remove(e);

        int count = 0;
        foreach(Node n in nodes)
        {
            if((n.Edges.Count) > CountForRandomRemove)
            {
                for(int i = n.Edges.Count - 1; i >= 0; --i)
                {
                    if(Random.value < RemoveChance)
                    {
                        count++;
                        Edge e = n.Edges[i];
                        n.Edges.RemoveAt(i);
                        edges.Remove(e);
                        for (int j = e.To.Edges.Count - 1; j >= 0; j--)
                        {
                            Edge et = e.To.Edges[j];
                            if (et.To == e.From && et.From == e.To)
                            {
                                Edge e1 = e.To.Edges[j];
                                e.To.Edges.RemoveAt(j);
                                edges.Remove(e1);
                                Destroy(et.gameObject);
                                break;
                            }
                        }
                        Destroy(e.gameObject);

                        if ((n.Edges.Count) <= CountForRandomRemove)
                            break;
                    }
                }
            }
        }
        Debug.Log("Removed " + count + " edges");

        HashSet<Node> visited = new HashSet<Node>();
        HashSet<Node> open = new HashSet<Node>();
        open.Add(nodes[0]);
        while(open.Count > 0)
        {
            Node node = open.First();
            open.Remove(node);

            visited.Add(node);

            foreach(Edge e in node.Edges)
            {
                if(!visited.Contains(e.To))
                    open.Add(e.To);    
            }
        }
        if(visited.Count != nodes.Count)
        {
            Debug.Log("Broken");
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    public static Node GetUnoccupiedRandomNode()
    {
        List<Node> temp = Instance.nodes.OrderBy(x => Random.value).ToList();
        foreach(Node n in temp)
        {
            if (n.Occupants.Count == 0)
                return n;
        }
        return null;
    }

    public static Node GetNodeUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100000.0f, Instance.NodeLayer))
            return hit.collider.gameObject.GetComponent<Node>();
        return null;
    }

    public static Node GetRandomNode()
    {
        return Instance.nodes[Random.Range(0, Instance.nodes.Count)];
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

    public List<Node> ShortestPath(Node start, Node target)
    {
        Dictionary<Node, float> distances = new Dictionary<Node, float>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> searched = new HashSet<Node>();

        foreach (Node n in nodes)
        {
            distances.Add(n, float.PositiveInfinity);
            previous.Add(n, null);
        }

        distances[start] = 0f;
        queue.Enqueue(start);

        while (queue.Count != 0)
        {
            Node current = queue.Dequeue();

            foreach (Node node in current.Edges.Select(e => e.To))
            {
                float distance = Vector3.Distance(current.transform.position, node.transform.position);
                float newDist = distances[current] + distance;
                if (newDist < distances[node])
                {
                    distances[node] = newDist;
                    previous[node] = current;
                }
                if (!queue.Contains(node) && !searched.Contains(node))
                {
                    queue.Enqueue(node);
                }
            }
            searched.Add(current);
        }

        List<Node> path = new List<Node>();
        Node curr = target;
        while (curr != start)
        {
            path.Add(curr);
            curr = previous[curr];
        }
        path.Reverse();
        return path;
    }
}

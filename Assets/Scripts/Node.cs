using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Edge> Edges = new List<Edge>();
    public List<Entity> Occupants = new List<Entity>();

    public void AddOccupant(Entity obj)
    {
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        Occupants.Add(obj);
        obj.Node = this;
    }
    public void RemoveOccupant(Entity obj)
    {
        Occupants.Remove(obj);
        obj.transform.parent = null;
    }
    public override string ToString()
    {
        return gameObject.name;
    }
}

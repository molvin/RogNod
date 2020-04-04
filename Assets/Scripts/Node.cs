using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Edge> Edges = new List<Edge>();
    public List<GameObject> Occupants = new List<GameObject>();

    public void AddOccupant(GameObject obj)
    {
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        Occupants.Add(obj);
    }
    public void RemoveOccupant(GameObject obj)
    {
        Occupants.Remove(obj);
        obj.transform.parent = null;
    }
    public override string ToString()
    {
        return gameObject.name;
    }
}

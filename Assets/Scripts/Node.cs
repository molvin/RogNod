using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Edge> Edges = new List<Edge>();
    public List<Entity> Occupants = new List<Entity>();
    public SpriteRenderer Renderer;

    [Header("Graphics")]
    public Sprite DefaultTile;
    public Sprite RedTile;
    public Sprite YellowTile;
    public Sprite BlueTile;
    public float OccupantHeight;

    public void AddOccupant(Entity obj)
    {
        obj.transform.parent = transform;
        obj.transform.localPosition = new Vector3(0, OccupantHeight, 0);
        Occupants.Add(obj);
        obj.Node = this;
    }
    public void RemoveOccupant(Entity obj)
    {
        Occupants.Remove(obj);
        obj.transform.parent = null;
    }

    public void ResetTileGrapic()
    {
        Renderer.sprite = DefaultTile;
    }

    public void MarkTileRed()
    {
        Renderer.sprite = RedTile;
    }

    public void MarkTileYellow()
    {
        Renderer.sprite = YellowTile;
    }
    public void MarkTileBlue()
    {
        Renderer.sprite = BlueTile;
    }

    public override string ToString()
    {
        return gameObject.name;
    }
}

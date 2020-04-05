using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Edge> Edges = new List<Edge>();
    public List<Entity> Occupants = new List<Entity>();
    public SpriteRenderer Renderer;

    [Header("Graphics")]
    public List<Sprite> DefaultTile = new List<Sprite>();
    public List<Sprite> RedTile = new List<Sprite>();
    public List<Sprite> YellowTile = new List<Sprite>();
    public List<Sprite> BlueTile = new List<Sprite>();
    private int doodad;
    private Marker lastAdded;
    private int[] markers;
    public enum Marker{ Red, Blue, Yellow, Default }

    public void Start()
    {
        doodad = Random.Range(0, 2);
        markers = new int[3];
    }

    public void Update()
    {
        //Pick default
        if (markers[0] == 0 && markers[1] == 0 && markers[2] == 0)
        {
            SetToColor(Marker.Default);
            return;
        }

        //Pick last added
        if (markers[(int)lastAdded] > 0)
        {
            SetToColor(lastAdded);
            return;
        }

        //Pick Red
        if (markers[0] >= markers[1] && markers[0] >= markers[2])
        {
            SetToColor(Marker.Red);
            return;
        }
        //Pick Blue
        if (markers[1] >= markers[0] && markers[1] >= markers[2])
        {
            SetToColor(Marker.Blue);
            return;
        }
        //Pick Yellow
        if (markers[2] >= markers[0] && markers[2] >= markers[1])
        {
            SetToColor(Marker.Yellow);
            return;
        }
    }

    private void SetToColor(Marker color)
    {
        if ((int)color == 0)
            Renderer.sprite = RedTile[doodad];
        if ((int)color == 1)
            Renderer.sprite = BlueTile[doodad];
        if ((int)color == 2)
            Renderer.sprite = YellowTile[doodad];
        if ((int)color == 3)
            Renderer.sprite = DefaultTile[doodad];
    }

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

    public void MarkTile(Marker color)
    {
        markers[(int)color]++;
    }

    public void DemarkTile(Marker color)
    {
        lastAdded = color;
        markers[(int)color]--;
        if (markers[(int)color] < 0)
            markers[(int)color] = 0;
    }

    public override string ToString()
    {
        return gameObject.name;
    }
}

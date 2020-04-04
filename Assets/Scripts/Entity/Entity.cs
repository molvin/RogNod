using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected int health;
    public abstract int Health { get; set; }
    public Node Node;
}

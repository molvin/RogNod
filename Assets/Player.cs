using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public override int Health { get => health; set
        {
            health = value;
            if (health <= 0)
            {
                GameLoop.Instance.GameOver();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Entity
{
    public TextMeshProUGUI HpText;

    public override int Health { get => health; set
        {
            health = value;
            HpText.text = Mathf.Max(health, 0).ToString();
        }
    }
}

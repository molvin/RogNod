using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICard : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;

    public void Set(Card c)
    {
        Title.text = c.getTitle();
        Title.text += $" ({c.Cost})";

        Description.text = c.getDescription();
    }
}

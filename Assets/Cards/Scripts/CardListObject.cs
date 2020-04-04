using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardListObject : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField]
    private TextMeshProUGUI _titleTextObject;
#pragma warning restore CS0649

    public void SetData(string title)
    {
        _titleTextObject.SetText(title);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDeck : MonoBehaviour
{

    private Deck deck;
    public GameObject cardListItemPrefab;
    public GameObject contentParent;

    void Start()
    {
        deck = Persistance.Instance.Deck;
        deck.OnPlayDeckUpdate = BuildDeckList;
    }

    private void BuildDeckList(Queue<Card> cards)
    {
        foreach (Transform child in contentParent.transform)
        {
            Destroy(child.gameObject);
        }
 
        foreach (Card c in cards)
        {
            GameObject carListItem = Instantiate(cardListItemPrefab);
            carListItem.SetActive(true);
            carListItem.GetComponent<CardListObject>().SetData(c.getTitle() + "(" + c.Cost + ")");
            carListItem.transform.SetParent(contentParent.transform, false);
        }

    }
}

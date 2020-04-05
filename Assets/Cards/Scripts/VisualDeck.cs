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
        BuildDeckList(); 
    }

    private void BuildDeckList()
    {
        foreach (Transform child in contentParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        List<Card> cards = deck.getAllPermanetsCards();
 
        foreach (Card c in cards)
        {
            GameObject carListItem = Instantiate(cardListItemPrefab);
            carListItem.SetActive(true);
            carListItem.GetComponent<CardListObject>().SetData(c.getTitle() + "(" + c.Cost + ")");
            carListItem.transform.SetParent(contentParent.transform, false);
        }

    }
}

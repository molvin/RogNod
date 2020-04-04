using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickCard : MonoBehaviour
{
    public Button NextLevelButton;
    public Deck DrawDeck;
    public Deck Deck;
    public Transform CardsParent;
    public Button CardPrefab;
    public int DrawCount = 3;
    private List<Card> Cards;
    public bool CanPick = true;

    private void Start()
    {
        DrawDeck = Persistance.Instance.DrawDeck;
        Deck = Persistance.Instance.Deck;
        NextLevelButton.onClick.AddListener(NextLevel);
        for(int i = 0; i < DrawCount; i++)
        {
            DrawDeck.Shuffle();
            Card c = DrawDeck.DrawCard();
            if (c == null)
                continue;
            DrawDeck.RemovePermanent(c);
            Button newButton = Instantiate(CardPrefab, CardsParent);
            newButton.GetComponent<UICard>().Set(c);
            Card c1 = c;
            newButton.onClick.AddListener(() => Pick(c));
        }
    }
    private void Pick(Card c)
    {
        if (!CanPick)
            return;
        //TODO: do visualization, animation and shit
        Deck.AddPermant(c);
        CanPick = false;
    }
    private void NextLevel()
    {
        SceneManager.LoadScene(1);
    }
}

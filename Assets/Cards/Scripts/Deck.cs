using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu()]
public class Deck : ScriptableObject
{
#pragma warning disable CS0649
    [SerializeField]
    private Card[] _startingDeck;
    private List<Card> _permanentDeck;
#pragma warning restore CS0649

    private Queue<Card> _cardQueue;

    private List<Card> hand;
    private int handSize = 3;

    public void Initialize()
    {
        hand = null;
        _cardQueue = null;
        _permanentDeck = null;
        _permanentDeck = new List<Card>(_startingDeck);
        _cardQueue = new Queue<Card>(_permanentDeck);
        hand = new List<Card>();
        Shuffle();

    }

    public void AddPermant(Card card)
    {
        _permanentDeck.Add(card);
    }

    public void AddPermant(Card[] cards)
    {
        _permanentDeck.AddRange(cards);
    }

    public void RemovePermanent(Card card)
    {
        _permanentDeck.Remove(card);
    }

    public List<Card> getAllPermanetsCards()
    {
        return _permanentDeck;
    }

    public void ReinsertCard(Card card)
    {
        _cardQueue.Enqueue(card);
    }

    public Card[] DrawCards(int amount)
    {
        Card[] toReturn = new Card[amount];
        for (int i = 0; i < amount; i++)
        {
            toReturn[i] = _cardQueue.Dequeue();
        }
        return toReturn;
    }

    public Card DrawCard()
    {
        return _cardQueue.Dequeue();
    }

    public void refillHand()
    {
        int missingCards = handSize - hand.Count;
        if (missingCards == 0)
            return;
        Card[] newCards = DrawCards(missingCards);
    }

    public void addToHand(Card card)
    {
        hand.Add(card);
    }

    public void addToHand(Card[] cards)
    {
        hand.AddRange(cards);
    }

    public List<Card> getHand()
    {
        return hand;
    }

    public void playCardFromHand(Card card)
    {
        hand.Remove(card);
    }

    private void Shuffle()
    {
        _cardQueue.OrderBy(x => Random.value);
    }

}

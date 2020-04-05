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
    public int handSize = 3;

    public System.Action<List<Card>> OnHandUpdate;
    public System.Action<Queue<Card>> OnPlayDeckUpdate;

    public void Initialize()
    {
        OnHandUpdate = null;
        OnPlayDeckUpdate = null;
        _permanentDeck = new List<Card>(_startingDeck);
        _cardQueue = new Queue<Card>(_permanentDeck);
        hand = new List<Card>();
        Shuffle();

    }
    public void UpdateDeck()
    {
        _cardQueue = new Queue<Card>(_permanentDeck);
        Shuffle();
    }
    public void ClearHand()
    {
        foreach (Card c in hand)
            ReinsertCard(c);
        hand.Clear();
        OnHandUpdate?.Invoke(hand);
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
        OnPlayDeckUpdate?.Invoke(_cardQueue);
    }

    public List<Card> DrawCards(int amount)
    {
        List<Card> toReturn = new List<Card>();
        for (int i = 0; i < amount && _cardQueue.Count > 0; i++)
        {
            toReturn.Add(_cardQueue.Dequeue());
        }
        OnPlayDeckUpdate?.Invoke(_cardQueue);
        return toReturn;
    }

    public Card DrawCard()
    {
        OnPlayDeckUpdate?.Invoke(_cardQueue);
        return _cardQueue.Count == 0 ? null : _cardQueue.Dequeue();
    }

    public void refillHand()
    {
        Shuffle();
        int missingCards = handSize - hand.Count;
        if (missingCards == 0)
            return;
        addToHand(DrawCards(missingCards));
    }

    public void addToHand(Card card)
    {
        hand.Add(card);
        OnHandUpdate?.Invoke(hand);
    }

    public void addToHand(List<Card> cards)
    {
        hand.AddRange(cards);
        OnHandUpdate?.Invoke(hand);
    }

    public List<Card> getHand()
    {
        return hand;
    }

    public void playCardFromHand(Card card)
    {
        bool removed = hand.Remove(card);
        _cardQueue.Enqueue(card);
        OnHandUpdate?.Invoke(hand);
        OnPlayDeckUpdate?.Invoke(_cardQueue);
    }

    public void Shuffle()
    {
        _cardQueue = new Queue<Card>(_cardQueue.OrderBy(x => Random.value));
        OnPlayDeckUpdate?.Invoke(_cardQueue);
    }

}

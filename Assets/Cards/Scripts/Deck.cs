﻿using System.Collections;
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
        OnPlayDeckUpdate?.Invoke(_cardQueue);
    }

    public List<Card> DrawCards(int amount)
    {
        List<Card> toReturn = new List<Card>();
        for (int i = 0; i < amount; i++)
        {
            if(_cardQueue.Count == 0)
                _cardQueue = new Queue<Card>(_permanentDeck);
            if (_cardQueue.Count == 0)
                break;
            toReturn.Add(_cardQueue.Dequeue());
        }
        OnPlayDeckUpdate?.Invoke(_cardQueue);
        return toReturn;
    }

    public Card DrawCard()
    {
        OnPlayDeckUpdate?.Invoke(_cardQueue);
        return _cardQueue.Dequeue();
    }

    public void refillHand()
    {
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
        Debug.Log(hand.Count);

        OnHandUpdate?.Invoke(hand);
    }

    private void Shuffle()
    {
        _cardQueue = new Queue<Card>(_cardQueue.OrderBy(x => Random.value));
    }

}

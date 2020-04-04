using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistance : MonoBehaviour
{
    public static Persistance Instance;
    public Deck Deck;
    public Deck DrawDeck;

    private void Start()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Deck = Instantiate(Deck);
        DrawDeck = Instantiate(DrawDeck);
        Deck.Initialize();
        DrawDeck.Initialize();
    }
}

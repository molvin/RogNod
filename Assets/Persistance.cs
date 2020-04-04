using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistance : MonoBehaviour
{
    public static Persistance Instance;
    public Deck Deck;
    public Deck DrawDeck;
    public int EnemyCountIncrease = 1;
    public int NodeCountIncrease = 1;
    public int Round = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Deck = Instantiate(Deck);
            DrawDeck = Instantiate(DrawDeck);
            Deck.Initialize();
            DrawDeck.Initialize();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Debug.Log("ASDSDASDASDASDASD");
    }

}

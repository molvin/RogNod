using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Deck Deck;
    public Card MoveCard;
    public Transform HandParent;
    public Button ButtonPrefab;

    public Node HoveredNode;
    public Card SelectedCard;

    private List<Button> hand = new List<Button>();

    private void Start()
    {
        Deck.OnHandUpdate += UpdateHand;
        Deck.Initialize();
        Deck.refillHand();
    }

    private void Update()
    {
        Node temp = HoveredNode;
        HoveredNode = Graph.GetNodeUnderMouse();
        if(temp != HoveredNode)
        {
            if (temp != null)
            {
                //New hover
                Debug.Log("New hover");
            }
            else
                Debug.Log("Lost hover");
        }


        if(Input.GetMouseButtonDown(0))
        {
            if (HoveredNode != null)
            {
                CreateAction();            
            }
            else
                Debug.Log("Clicked nothing");
        }
    }
    
    private void CreateAction()
    {
        if (SelectedCard == null)
        {

            return;
        }

        FunctionAction action = SelectedCard.getAction();
        action.Origin = GameLoop.Instance.Player.Node;
        action.Target = HoveredNode;
        action.Initialize(GameLoop.Instance.Player);
        GameLoop.Instance.PlayerState.SetAction(action);
    }

    private void CreateMoveAction()
    {
        //Move instance = Instantiate(MoveAction);
        //instance.target = HoveredNode;
        //instance.Initialize(GameLoop.Instance.Player);
        //GameLoop.Instance.PlayerState.SetAction(instance);
    }
    
    private void UpdateHand(List<Card> cards)
    {
        foreach(Button b in hand)
        {
            Destroy(b.gameObject);
        }
        hand.Clear();

        cards.Add(MoveCard);
        cards.Reverse();
        foreach(Card c in cards)
        {
            Button newButton = Instantiate(ButtonPrefab, HandParent);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = c.getTitle();
            Card c1 = c;
            newButton.onClick.AddListener(() => SelectCard(c1));
            hand.Add(newButton);
        }
        SelectCard(cards[0]);
    }
    private void SelectCard(Card c)
    {
        SelectedCard = c;
    }

    private void OnGUI()
    {
        GUILayout.Label(SelectedCard == null ? "No selected card" : SelectedCard.getTitle());
    }
}

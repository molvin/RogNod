using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerUI : MonoBehaviour
{
    public Deck Deck;
    public Card MoveCard;
    public Transform HandParent;
    public Button ButtonPrefab;

    public Node HoveredNode;
    public Card SelectedCard;

    private List<Button> hand = new List<Button>();
    private FunctionAction pendingAction = null;
    private Coroutine VisualizeCoroutine;

    private void Start()
    {
        Deck.OnHandUpdate += UpdateHand;
        Deck.Initialize();
        Deck.refillHand();
    }

    private void Update()
    {
        if (GameLoop.Instance.PlayerState.Executing)
            return;

        Node temp = HoveredNode;
        HoveredNode = Graph.GetNodeUnderMouse();

        if(temp != HoveredNode)
        {
            if(HoveredNode != null)
                CreateAction();
            else if(pendingAction != null)
            {
                if (VisualizeCoroutine != null)
                {
                    StopCoroutine(VisualizeCoroutine);
                    VisualizeCoroutine = null;
                    pendingAction.ResetVisualization();
                }
                pendingAction = null;
            }

        }


        if (Input.GetMouseButtonDown(0) && pendingAction != null)
        {
            PerformAction();
        }
    }

    private void CreateAction()
    {
        //Check if target type resolves
        bool valid = false;
        if(SelectedCard.Target == Card.TargetType.Adjacent)
        {
            Node origin = GameLoop.Instance.Player.Node;
            if (HoveredNode.Edges.Any(e => e.To == origin))
                valid = true;
        }
        else if(SelectedCard.Target == Card.TargetType.Any)
        {
            valid = true;
        }

        if (!valid)
        {
            //TODO: visualize?
            if (VisualizeCoroutine != null)
            {
                StopCoroutine(VisualizeCoroutine);
                pendingAction.ResetVisualization();
            }
            pendingAction = null;
            return;
        }

        FunctionAction action = Instantiate(SelectedCard.getAction());
        action.Origin = GameLoop.Instance.Player.Node;
        action.Target = HoveredNode;
        action.Initialize(GameLoop.Instance.Player);

        //TODO: visualize

        if (VisualizeCoroutine != null)
        {
            StopCoroutine(VisualizeCoroutine);
            pendingAction.ResetVisualization();
        }

        pendingAction = action;


        VisualizeCoroutine = StartCoroutine(Visualize());
    }
    
    private void PerformAction()
    {
        if (SelectedCard == null || pendingAction == null)
        {

            return;
        }
        pendingAction.ResetVisualization();
        GameLoop.Instance.PlayerState.SetAction(pendingAction);
        GameLoop.Instance.PlayerState.Executing = true;
        HoveredNode = null;
        pendingAction = null;
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

    private IEnumerator Visualize()
    {
        yield return pendingAction.Visualize();
        VisualizeCoroutine = null;
    }

    private void OnGUI()
    {
        GUILayout.Label(SelectedCard == null ? "No selected card" : SelectedCard.getTitle());
    }
}

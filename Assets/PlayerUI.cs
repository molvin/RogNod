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

    public int CurrentEnergy;
    public int MaxEnergy = 3;

    public Button EndTurnButton;
    private bool inState = true;

    private void Start()
    {
        CurrentEnergy = MaxEnergy;
        Deck.OnHandUpdate += UpdateHand;
        Deck.Initialize();
        Deck.refillHand();

        EndTurnButton.onClick.AddListener(EndTurn);
    }

    private void Update()
    {
        if (GameLoop.Instance.PlayerState.Executing || !GameLoop.Instance.PlayerState.InState)
            return;

        if(!inState)
        {
            inState = true;
            Deck.refillHand();
        }

        if (SelectedCard == null)
            return;
        if (SelectedCard.Cost > CurrentEnergy)
        {
            SelectedCard = null;
            return;
        }

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
        CurrentEnergy -= SelectedCard.Cost;
        Deck.playCardFromHand(SelectedCard);
        SelectedCard = null;
    }
    private void UpdateHand(List<Card> cards)
    {
        foreach(Button b in hand)
        {
            Destroy(b.gameObject);
        }
        hand.Clear();
        List<Card> cs = new List<Card>(cards);
        cs.Add(MoveCard);
        cs.Reverse();
        foreach(Card c in cs)
        {
            Button newButton = Instantiate(ButtonPrefab, HandParent);
            newButton.GetComponent<UICard>().Set(c);
            Card c1 = c;
            newButton.onClick.AddListener(() => SelectCard(c1));
            hand.Add(newButton);
        }
    }
    private void SelectCard(Card c)
    {
        if (CurrentEnergy < c.Cost)
            return;
        SelectedCard = c;
        pendingAction = null;
    }
    private void EndTurn()
    {
        GameLoop.Instance.PlayerState.EndTurn = true;
        CurrentEnergy = MaxEnergy;
        inState = false;
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

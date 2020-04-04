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

    public Transform EnergyParent;
    public Image EnergyPrefab;
    private List<Image> Energy = new List<Image>();

    public Button EndTurnButton;
    private bool inState = true;
    private bool won = false;
    public Animator Anim;

    public TextMeshProUGUI Text;

    private void Start()
    {
        Deck = Persistance.Instance.Deck;
        CurrentEnergy = MaxEnergy;
        Deck.OnHandUpdate = UpdateHand;    
        Deck.refillHand();


        EndTurnButton.onClick.AddListener(EndTurn);

        for(int i = 0; i < MaxEnergy; i++)
        {
            Energy.Add(Instantiate(EnergyPrefab, EnergyParent));
        }
        Text.text = "";
    }

    private void Update()
    {
        if(GameLoop.Instance.CurrentState is PickCardState)
        {
            if(!won)
            {
                won = true;
                Anim.SetBool("PickCard", true);
            }
            return;
        }


        if (GameLoop.Instance.PlayerState.Executing || !GameLoop.Instance.PlayerState.InState)
            return;

        if(!inState)
        {
            inState = true;
            Deck.refillHand();
            for (int i = 0; i < MaxEnergy; i++)
            {
                Energy[i].enabled = (i < CurrentEnergy);
            }
        }

        if (SelectedCard == null)
            return;
        if (SelectedCard.Cost > CurrentEnergy)
        {
            SelectedCard = null;
            return;
        }
        if (SelectedCard.Target == Card.TargetType.None)
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
        if (SelectedCard.Target == Card.TargetType.Adjacent)
        {
            Node origin = GameLoop.Instance.Player.Node;
            if (HoveredNode.Edges.Any(e => e.To == origin))
                valid = true;
        }
        else if (SelectedCard.Target == Card.TargetType.Any)
        {
            valid = true;
        }
        else if (SelectedCard.Target == Card.TargetType.None)
            valid = true;
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
            Debug.Log("DOING IT: " + SelectedCard + " " + pendingAction);

            return;
        }
    
        pendingAction.ResetVisualization();
        GameLoop.Instance.PlayerState.SetAction(pendingAction);
        GameLoop.Instance.PlayerState.Executing = true;
        HoveredNode = null;
        pendingAction = null;
        CurrentEnergy -= SelectedCard.Cost;
        for(int i = 0; i < MaxEnergy; i++)
        {
            Energy[i].enabled = (i < CurrentEnergy);
        }
        Deck.playCardFromHand(SelectedCard);
        SelectedCard = null;
        Text.text = "";
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
        {
            SelectedCard = null;
            Text.text = "";
            return;
        }
        if(SelectedCard != null && SelectedCard.Target == Card.TargetType.None && c == SelectedCard)
        {
            PerformAction();
            return;
        }
        SelectedCard = c;

        switch (SelectedCard.Target)
        {
            case Card.TargetType.Adjacent:
                Text.text = "Click an adjacent node to act";
                break;
            case Card.TargetType.Any:
                Text.text = "Click any node to act";
                break;
            case Card.TargetType.None:
                Text.text = "Click card again to act";
                CreateAction();

                return;
        }

        pendingAction = null;
    }
    private void EndTurn()
    {
        GameLoop.Instance.PlayerState.EndTurn = true;
        GameLoop.Instance.PlayerState.InState = false;
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

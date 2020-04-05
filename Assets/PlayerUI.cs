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
    private bool lost = false;
    public Animator Anim;

    public TextMeshProUGUI Text;

    private int index;

    public Button MainMenuButton;

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
        MainMenuButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
    }

    private void Update()
    {
        if(GameLoop.Instance.RageQuitState.RageQuit)
        {
            if(!lost)
            {
                lost = true;
                Anim.SetBool("Lost", true);              
            }
            return;
        }

        if(GameLoop.Instance.CurrentState is PickCardState)
        {
            if(!won)
            {
                won = true;
                Deck.ClearHand();
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
                }
                pendingAction.ResetVisualization();
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
                if (pendingAction != null)
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
            if(pendingAction != null)
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
        EnergyParent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        for (int i = 0; i < MaxEnergy; i++)
        {
            Energy[i].enabled = (i < CurrentEnergy);
        }
        if(SelectedCard != MoveCard)
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
        List<Card> cs = new List<Card>();
        foreach (Card c in cards)
            cs.Add(c);
        cs.Add(MoveCard);
        cs.Reverse();
        int i = 0;
        foreach(Card c in cs)
        {
            Button newButton = Instantiate(ButtonPrefab, HandParent);
            newButton.GetComponent<UICard>().Set(c);
            Card c1 = c;
            int j = i;
            newButton.onClick.AddListener(() => SelectCard(c1, j));
            hand.Add(newButton);
            i++;
        }
    }
    private void SelectCard(Card c, int i)
    {
        if (!inState || GameLoop.Instance.PlayerState.Executing)
            return;

        if (CurrentEnergy < c.Cost)
        {
            SelectedCard = null;
            Text.text = "";
            return;
        }
        if(SelectedCard != null && SelectedCard.Target == Card.TargetType.None && c == SelectedCard && i == index)
        {
            PerformAction();
            return;
        }
        if(SelectedCard != null && i != index)
        {
            if(pendingAction != null)
            {
                if(VisualizeCoroutine != null)
                    StopCoroutine(VisualizeCoroutine);
                pendingAction.ResetVisualization();
            }
        }
        SelectedCard = c;
        index = i;

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
        Text.text = "";
        Deck.ClearHand();
        
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

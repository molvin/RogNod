using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/PlayerState")]
public class PlayerState : GameLoopState
{
    public bool Executing = false;
    public bool InState = false;
    public bool EndTurn = false;
    private FunctionAction action;
    public override IEnumerator Enter()
    {
        GameLoop.RoundText.text = "Player Turn";
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Entered");
        Executing = false;
        EndTurn = false;
        InState = true;
        yield return null;
    }
    public override IEnumerator Run()
    {
        if (Executing)
        {
            if (!GameLoop.Player.Stunned)
                yield return action.Act();
            else
                GameLoop.Player.Stunned = false;
            action = null;
            Executing = false;
        }

        if (GameLoop.enemies.Count == 0)
            stateMachine.ChangeState<PickCardState>();
        else if (EndTurn)
            stateMachine.ChangeState<EnemyResolveState>();
    }
    public override IEnumerator Exit()
    {
        InState = false;
        Debug.Log("Exit Player State");
        yield return null;
    }
    public void SetAction(FunctionAction action)
    {
        this.action = action;
        //this.action = Instantiate(action);
        //this.action.Initialize(GameLoop.Player);
    }
}
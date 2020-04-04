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
        //Debug.Log("Waiting 3 seconds");
        //yield return new WaitForSeconds(3f);
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
            yield return action.Act();
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/PlayerState")]
public class PlayerState : GameLoopState
{
    private FunctionAction action;
    public override IEnumerator Enter()
    {
        //Debug.Log("Waiting 3 seconds");
        //yield return new WaitForSeconds(3f);
        Debug.Log("Entered");
        yield return null;
    }
    public override IEnumerator Run()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            yield return action.Act();
            action = null;
            stateMachine.ChangeState<EnemyResolveState>();
        }
        yield return null;
    }
    public override IEnumerator Exit()
    {
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
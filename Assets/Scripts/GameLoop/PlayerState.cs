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
    public override void Run()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState<EnemyResolveState>();
        }
    }
    public override IEnumerator Exit()
    {
        yield return action.Act();
    }
    public void SetAction(FunctionAction action)
    {
        this.action = Instantiate(action);
        this.action.Initialize(GameLoop.Player);
    }
}
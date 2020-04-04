using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/PlayerState")]
public class PlayerState : GameLoopState
{
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
            stateMachine.ChangeState<EnemyDecideState>();
        }
    }
}

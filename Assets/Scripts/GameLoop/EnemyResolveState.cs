using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/EnemyResolveState")]
public class EnemyResolveState : GameLoopState
{
    public override IEnumerator Enter()
    {
        foreach (Enemy enemy in GameLoop.enemies)
        {
            if (!enemy.Stunned)
                yield return enemy.action.Act();
            else
                enemy.Stunned = false;

            if (GameLoop.Player.Health <= 0)
                break;
        }
        if (GameLoop.Player.Health <= 0)
            stateMachine.ChangeState<RageQuitStage>();
        else
            stateMachine.ChangeState<EnemyDecideState>();
    }
}
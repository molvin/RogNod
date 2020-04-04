using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/EnemyDecideState")]
public class EnemyDecideState : GameLoopState
{
    public override IEnumerator Enter()
    {
        foreach (Enemy enemy in GameLoop.enemies)
        {
            FunctionAction action = enemy.PickAction();
            yield return enemy.StubVisualize();
        }
        stateMachine.ChangeState<PlayerState>();
    }
}

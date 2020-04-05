using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/EnemyResolveState")]
public class EnemyResolveState : GameLoopState
{
    public override IEnumerator Enter()
    {
        GameLoop.RoundText.text = "Enemies Executing";
        yield return new WaitForSeconds(0.5f);

        List<Enemy> enemies = GameLoop.enemies.GetRange(0, GameLoop.enemies.Count);
        foreach (Enemy enemy in enemies)
        {
            if (!GameLoop.enemies.Contains(enemy))
                continue;
            enemy.action.ResetVisualization();

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
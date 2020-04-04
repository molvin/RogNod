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
            Edge e = enemy.Node.Edges[Random.Range(0, enemy.Node.Edges.Count)];
            Node target = e.To;
            yield return action.Visualize(enemy.gameObject, enemy.Node, target);
        }
        stateMachine.ChangeState<PlayerState>();
    }
}

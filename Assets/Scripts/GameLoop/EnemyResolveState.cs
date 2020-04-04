﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/EnemyResolveState")]
public class EnemyResolveState : GameLoopState
{
    public override IEnumerator Enter()
    {
        foreach (Enemy enemy in GameLoop.enemies)
        {
            yield return enemy.action.Act();
        }
        stateMachine.ChangeState<EnemyDecideState>();
    }
}
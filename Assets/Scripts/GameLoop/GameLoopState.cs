using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameLoopState : State
{
    private GameLoop gameLoop;
    protected GameLoop GameLoop => gameLoop = gameLoop ?? (GameLoop)owner;
}

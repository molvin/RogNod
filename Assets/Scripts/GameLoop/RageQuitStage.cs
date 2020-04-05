using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/RageQuitStage")]
public class RageQuitStage : GameLoopState
{
    public bool RageQuit;
    public float WaitTime = 3.0f;

    public override IEnumerator Enter()
    {
        RageQuit = true;
        yield return null;

    }
    public override IEnumerator Exit()
    {
        RageQuit = false;
        yield return null;

    }
}

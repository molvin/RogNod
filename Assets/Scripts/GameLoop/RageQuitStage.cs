using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/RageQuitStage")]
public class RageQuitStage : GameLoopState
{
    public override IEnumerator Enter()
    {
        Debug.Log("QUIT");
        yield return null;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

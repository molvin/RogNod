using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameLoop/PickCardState")]
public class PickCardState : GameLoopState
{
    public override IEnumerator Run()
    {
        Debug.Log("WINNER WINNER CHICKEN DINNER");
        yield return null;
    }
}

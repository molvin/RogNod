using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActiontTest : MonoBehaviour
{
    public void AddAction(FunctionAction action)
    {
        GameLoop.Instance.PlayerState.SetAction(action);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public FunctionAction action;

    public FunctionAction PickAction()
    {
        return action;
    }
    public IEnumerator StubVisualize()
    {
        float time = 0;
        while (time < 1f)
        {
            Debug.Log("Visualizing: " + transform.name);
            time += Time.deltaTime;
            yield return null;
        }
    }
}

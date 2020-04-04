using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private FunctionAction actionAsset;
    [HideInInspector] public FunctionAction action;
    public Node Node;

    public FunctionAction PickAction()
    {
        action = Instantiate(actionAsset);
        action.Initialize(this);
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

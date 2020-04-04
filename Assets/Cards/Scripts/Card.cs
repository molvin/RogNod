using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Card : ScriptableObject
{
#pragma warning disable CS0649
    [SerializeField]
    private string Title;
    [SerializeField]
    private string Description;
    [SerializeField]
    private FunctionAction Action; 
    [SerializeField]
    private Sprite sprite;
#pragma warning restore CS0649


    public string getTitle()
    {
        return Title;
    }

    public string getDescription()
    {
        return Description;
    }

    public FunctionAction getAction()
    {
        return Action;
    }

    public Sprite getImage()
    {
        return sprite;
    }

}

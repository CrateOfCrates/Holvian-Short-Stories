using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public abstract class TokenObject
{
    [SerializeField] private Sprite display;
    [SerializeField] private string moveName;

    public Sprite Display => display;
    public string MoveName => moveName;

    
}

[Serializable]
public class SingleAttackObject : TokenObject
{
    public int test;
    
}



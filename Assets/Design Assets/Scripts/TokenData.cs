using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public abstract class TokenObject
{
    [SerializeField] private Sprite display;
    public Sprite Display => display;
}

[Serializable]
public class SingleAttackToken : TokenObject
{
    [SerializeField] private int attackPower;
    public int AttackPower => attackPower;
}

[Serializable]
public class SingleBlockToken : TokenObject
{
    [SerializeField] private int blockPower;
    public int BlockPower => blockPower;
}

[Serializable]
public class DelayToken : TokenObject
{
    
}

[Serializable]
public class EvadeToken : TokenObject
{
    [SerializeField] private int evadePower;
    public int EvadePower => evadePower;
    
}



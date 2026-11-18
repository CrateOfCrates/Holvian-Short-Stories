using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class TokenObject
{
    [SerializeField] private Sprite display;
    [SerializeField] private int potency;
    [SerializeField] TokenType tokenType;
    [SerializeField] Target tokenTarget;
    [SerializeField] AnimationObject animationPlayedTarget;

    [SerializeField] private string onUseText;
    [SerializeField] private string tokenDescription;

    [SerializeReference] [SubclassSelector] List<TokenParts> tokenParts;

    public int Potency => potency;
    public Sprite Display => display;
    public TokenType TokenType => tokenType;
    public Target TokenTarget => tokenTarget;
    public AnimationObject AnimationPlayedTarget => animationPlayedTarget;
    public List<TokenParts> TokenParts => tokenParts;
    public string TokenDescription => tokenDescription;

    public string GetTextDisplay(string userName)
    {
        string s = userName + " " + onUseText;
        return s;
    }
    
}

[System.Serializable]
public abstract class TokenParts
{
    [SerializeField] private TriggerTime triggerTime;

    public TriggerTime TriggerTime => triggerTime;

    public virtual void Execute(EntityData user, EntityData target){}
}

[System.Serializable]
public class ApplyDamageStatus : TokenParts
{
    [SerializeReference] private DamageSEO type;
    [SerializeField] private int statusPotency;

    public override void Execute(EntityData user, EntityData target)
    {
        target.ApplyStatus(type, statusPotency);
    }
}


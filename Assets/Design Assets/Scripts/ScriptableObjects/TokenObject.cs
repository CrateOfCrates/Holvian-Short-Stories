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
    [SerializeField] TokenTarget tokenTarget;
    [SerializeField] AnimationObject animationPlayedTarget;

    [SerializeReference] [SubclassSelector] List<TokenParts> tokenParts;

    public int Potency => potency;
    public Sprite Display => display;
    public TokenType TokenType => tokenType;
    public TokenTarget TokenTarget => tokenTarget;
    public AnimationObject AnimationPlayedTarget => animationPlayedTarget;
    public List<TokenParts> TokenParts => tokenParts;

    public bool NeedAim()
    {
        if(tokenTarget == TokenTarget.target)
        {
            return true;
        } else if (tokenTarget == TokenTarget.entireSide)
        {
            return true;
        } else
        {
            return false;
        }
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


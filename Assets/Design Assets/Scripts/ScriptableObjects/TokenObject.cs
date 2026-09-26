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

    public int Potency => potency;
    public Sprite Display => display;
    public TokenType TokenType => tokenType;
    public TokenTarget TokenTarget => tokenTarget;

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


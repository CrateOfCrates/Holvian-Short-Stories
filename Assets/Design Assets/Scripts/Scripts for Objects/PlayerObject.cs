using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Player Character")]
public class PlayerObject : EntityObject
{
    [Serializable]
    public struct ExpressionPortrait
    {
        public string expressionKey; // e.g. "surprised"
        public Sprite portraitSprite;
    }

    //Start
    

    [SerializeField] private int hp;
    [SerializeField] private int mp;
    [field: SerializeField] private List<ExpressionPortrait> expressions;
    [field: SerializeField] private Sprite characterBackground;

    //Getters
    public int Hp => hp;
    public int Mp => mp;

    public Sprite GetSprite(string name)
    {
        foreach(ExpressionPortrait ep in expressions)
        {
            if(name == ep.expressionKey)
            {
                return ep.portraitSprite;
            }
        }
        return null;
    }

     public Sprite CharacterBackground => characterBackground;
    

}

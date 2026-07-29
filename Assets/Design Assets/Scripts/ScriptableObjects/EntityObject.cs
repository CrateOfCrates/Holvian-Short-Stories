using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EntityObject : ScriptableObject
{

    [field: SerializeField] private string entityName;
    [SerializeField] private int baseMaxhp;
    [SerializeField] private int baseMaxmp;
    [SerializeField] private List<MoveObject> startingMoves;
    [field: SerializeField] private List<KeySprite> expressions;

    public string EntityName => entityName;
    public int BaseMaxHp => baseMaxhp;
    public int BaseMaxMp => baseMaxmp;
    public List<MoveObject> StartingMoves => startingMoves;
    
    public Sprite GetSprite(string name)
    {
        foreach(KeySprite ep in expressions)
        {
            if(name == ep.key)
            {
                return ep.sprite;
            }
        }
        return null;
    }


}

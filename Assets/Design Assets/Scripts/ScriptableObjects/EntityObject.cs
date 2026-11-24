using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EntityObject : ScriptableObject
{
    [Serializable]
    private struct SpriteEntry
    {
        [SerializeField] private string key;
        [SerializeField] private Sprite value;

        public string Key => key;
        public Sprite Value => value;
    }

    [field: SerializeField] private string entityName;

    [SerializeField] private int baseMaxhp;
    [SerializeField] private int baseMaxmp;

    [SerializeField] private int baseSpeed;
    [SerializeField] private int baseAttack;
    [SerializeField] private int baseDefense;
    [SerializeField] private int baseArmor;
    [SerializeField] private int startingLevel;

    [SerializeField] private List<MoveObject> startingMoves;
    [field: SerializeField] private List<SpriteEntry> expressions;

    public string EntityName => entityName;
    public int BaseMaxHp => baseMaxhp;
    public int BaseMaxMp => baseMaxmp;
    public int BaseSpeed => baseSpeed;
    public int BaseAttack => baseAttack;
    public int BaseDefense => baseDefense;
    public int BaseArmor => baseArmor;

    public int StartingLevel => startingLevel;

    public List<MoveObject> StartingMoves => startingMoves;
    
    public Sprite GetSprite(string name)
    {
        foreach(SpriteEntry ep in expressions)
        {
            if(name == ep.Key)
            {
                return ep.Value;
            }
        }
        return null;
    }


}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using System;
using TMPro;

[Serializable]
public struct KeySprite
    {
        public string key;
        public Sprite sprite;
        
    }


[System.Serializable]
public class EntityData
{
    [SerializeField] protected int hp;
    [SerializeField] protected int mp;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int maxMp;
    [SerializeField] protected List<MoveData> activeMoves;

    public int Hp => hp;
    public int Mp => mp;
    public int MaxHp => maxHp;
    public int MaxMp => maxMp;
    public List<MoveData> ActiveMoves => activeMoves;

    protected void LoadStats(int maxHp, int maxMp)
    {
        hp = maxHp;
        mp = maxMp;
        this.maxHp = maxHp;
        this.maxMp = maxMp;
    }

    protected void LoadMoves(List<MoveObject> moves){

        activeMoves = new List<MoveData>();

        for(int i = 0; i < moves.Count; i++)
        {
            activeMoves.Add(new());
            activeMoves[i].LoadData(moves[i]);
        }
    }

}

[System.Serializable]
public class PlayerData : EntityData
{
    [SerializeField] private PlayerObject baseInfo;

    public PlayerObject BaseInfo => baseInfo;

    public void LoadData(PlayerObject playerObject)
    {
        LoadStats(playerObject.BaseMaxHp, playerObject.BaseMaxMp);
        LoadMoves(playerObject.StartingMoves);
        baseInfo = playerObject;
    }

}

[System.Serializable]
public class EnemyData : EntityData
{
    [SerializeField] private EnemyObject baseInfo;
    [SerializeField] private SingleEnemyManager singleEnemyManager;

    public EnemyObject BaseInfo => baseInfo;
    public SingleEnemyManager SingleEnemyManager => singleEnemyManager;

    public void LoadData(EnemyObject enemyObject, SingleEnemyManager singleEnemyManager)
    {
        LoadStats(enemyObject.BaseMaxHp, enemyObject.BaseMaxMp);
        LoadMoves(enemyObject.StartingMoves);
        baseInfo = enemyObject;
        this.singleEnemyManager = singleEnemyManager;
    }
}

[System.Serializable]
public class MoveData
{
    [SerializeField] private List<TokenObject> activeTokens;
    [SerializeField] private MoveObject baseInfo;

    public List<TokenObject> ActiveTokens => activeTokens;
    public MoveObject BaseInfo => baseInfo;
    public string MoveName => baseInfo.MoveName;

    public void LoadData(MoveObject moveObject)
    {
        activeTokens = new List<TokenObject>(moveObject.BaseTokens);
        baseInfo = moveObject;
    }
}


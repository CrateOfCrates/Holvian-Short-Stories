using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using System;
using TMPro;

public enum WhichTeam {player, enemy, undec};

[Serializable]
public struct KeySprite
    {
        public string key;
        public Sprite sprite;
    }

[Serializable]
public class Targeting 
{
    [SerializeField] private int whichUser;
    [SerializeField] private WhichTeam userTeam;
    [SerializeField] private int whichTarget;
    [SerializeField] private WhichTeam targetTeam;
    [SerializeField] private int moveUsed;

    public int WhichUser => whichUser;
    public WhichTeam UserTeam => userTeam;
    public int WhichTarget => whichTarget;
    public WhichTeam TargetTeam => targetTeam;

    public int MoveUsed => moveUsed;

    public Targeting(){
        moveUsed = -1;
        whichUser = -1;
        whichTarget = -1;
        targetTeam = WhichTeam.undec;
        userTeam = WhichTeam.undec;
    }

    public Targeting(int moveUsed, int whichUser, WhichTeam userTeam, int whichTarget, WhichTeam targetTeam){
        this.moveUsed = moveUsed;
        this.whichUser = whichUser;
        this.userTeam = userTeam;
        this.whichTarget = whichTarget;
        this.targetTeam = targetTeam;
    }

    public void SetTarget(int whichTarget, WhichTeam targetTeam)
    {
        this.whichTarget = whichTarget;
        this.targetTeam = targetTeam;
    }

    public void SetTarget(int whichTarget)
    {
        this.whichTarget = whichTarget;
    }

    public void SetUser(int moveUsed, int whichUser, WhichTeam userTeam)
    {
        this.moveUsed = moveUsed;
        this.whichUser = whichUser;
        this.userTeam = userTeam;
    }
}

[System.Serializable]
public class EntityData
{
    [SerializeField] protected int hp;
    [SerializeField] protected int mp;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int maxMp;

    [SerializeField] protected int speed;

    [SerializeField] protected List<MoveData> activeMoves;
    
    public int Hp => hp;
    public int Mp => mp;
    public int MaxHp => maxHp;
    public int MaxMp => maxMp;

    public int Speed => speed;

    public List<MoveData> ActiveMoves => activeMoves;

    protected void LoadStats(int maxHp, int maxMp, int speed)
    {
        hp = maxHp;
        mp = maxMp;
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.speed = speed;
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
    [SerializeField] private bool exists;

    public PlayerObject BaseInfo => baseInfo;
    public bool Exists => exists;

    public void SetExists(bool b){exists = b;}

    public void LoadData(PlayerObject playerObject)
    {
        LoadStats(playerObject.BaseMaxHp, playerObject.BaseMaxMp, playerObject.BaseSpeed);
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
        LoadStats(enemyObject.BaseMaxHp, enemyObject.BaseMaxMp, enemyObject.BaseSpeed);
        LoadMoves(enemyObject.StartingMoves);
        baseInfo = enemyObject;
        this.singleEnemyManager = singleEnemyManager;
    }
}

[System.Serializable]
public class MoveData
{
    [SerializeField] private List<TokenObject> activeTokens;
    [SerializeField] private int mpCost;
    [SerializeField] private MoveObject baseInfo;

    public List<TokenObject> ActiveTokens => activeTokens;
    public int MpCost => mpCost;
    public MoveObject BaseInfo => baseInfo;
    public string MoveName => baseInfo.MoveName;

    public void LoadData(MoveObject moveObject)
    {
        activeTokens = new List<TokenObject>(moveObject.BaseTokens);
        mpCost = moveObject.MpCost;
        baseInfo = moveObject;
    }
}


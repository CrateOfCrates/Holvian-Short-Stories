using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Combat/Enemy")]
public class EnemyObject : EntityObject
{
    [SerializeField] private int actionCount;
    [SerializeReference] [SubclassSelector] private List<aiChoice> aiChoices;

    public int ActionCount => actionCount;
    public List<aiChoice> AiChoices => aiChoices;
}



[Serializable]
public class EnemyAiTargeting
{
    public int score;
    public Targeting targeting;
    public bool allowed;

    // public EnemyAiTargeting(int score, Targeting targeting)
    // {
    //     this.score = score;
    //     this.targeting = targeting;
    //     allowed = true;
    // }

    public EnemyAiTargeting(int score, int move, int t, WhichTeam w, bool allowed)
    {
        this.score = score;
        this.targeting = new Targeting(move, t, w);
        this.allowed = allowed;
    }

    public EnemyAiTargeting()
    {
        score = -1;
        targeting = null;
        allowed = false;
    }
}

[System.Serializable]
public class aiChoice
{
    [SerializeField] protected int bonusScore = 0;
    [SerializeField] protected int moveIndex;

    protected List<EnemyAiTargeting> MakeAllTargets(List<PlayerData> players, List<EnemyData> enemies, Dictionary<int, int> gd)
    {
        List<EnemyAiTargeting> tempList = new List<EnemyAiTargeting>();

        //add all players
        for(int i = 0; i < players.Count; i++)
        {
            EnemyAiTargeting temp = new EnemyAiTargeting(bonusScore+gd[i], moveIndex, i, WhichTeam.player, true);
            tempList.Add(temp);
        }
        //add for all enemies
        for(int i = 0; i < enemies.Count; i++)
        {
            EnemyAiTargeting temp = new EnemyAiTargeting(bonusScore, moveIndex, i, WhichTeam.enemy, false);
            tempList.Add(temp);
        }

        return tempList;
    }

    public virtual List<EnemyAiTargeting> Execute(int turn, EntityData self, List<PlayerData> players, List<EnemyData> enemies, Dictionary<int, int> gd)
    {
        return MakeAllTargets(players, enemies, gd);
    }
}

[System.Serializable]
public class TurnBasedActivation : aiChoice
{
    [SerializeField] protected int turnInterval;
    public override List<EnemyAiTargeting> Execute(int turn, EntityData self, List<PlayerData> players, List<EnemyData> enemies, Dictionary<int, int> gd)
    {
        if((turn+1) % turnInterval == 0)
        {
            return MakeAllTargets(players, enemies, gd);
        } else {return null;}
    }
}

[System.Serializable]
public class HpBasedActivation : aiChoice
{
    [SerializeField] protected double hpPrecent;
    public override List<EnemyAiTargeting> Execute(int turn, EntityData self, List<PlayerData> players, List<EnemyData> enemies, Dictionary<int, int> gd)
    {
        if(self.Hp <= self.MaxHp*hpPrecent)
        {
            return MakeAllTargets(players, enemies, gd);
        } else {return null;}
    }
}
